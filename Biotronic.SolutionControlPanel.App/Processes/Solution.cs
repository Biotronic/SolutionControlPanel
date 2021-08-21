using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Biotronic.SolutionControlPanel.App.Config;
using Biotronic.SolutionControlPanel.App.Utils;

namespace Biotronic.SolutionControlPanel.App.Processes
{
    internal abstract class Solution
    {
        private static readonly WeakList<Solution> AllSolutions = new WeakList<Solution>();

        public bool RedirectedOutput { get; private set; }
        private string _profile;
        public string Profile
        {
            get => _profile;
            protected set
            {
                value ??= Profiles.Keys.FirstOrDefault(a => Regex.IsMatch(a, Config.DefaultProfilePattern));
                _profile = value;
                SolutionConfig.Profile = value;
            }
        }

        private bool _checked;
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                SolutionConfig.Checked = value;
            }
        }

        private bool _hidden;
        public bool Hidden
        {
            get => _hidden;
            set
            {
                if (value == _hidden) return;
                _hidden = value;
                SolutionConfig.Hidden = value;
            }
        }
        public string SolutionPath { get; }
        public string Name { get; protected set; }
        public Dictionary<string, LaunchProfile> Profiles { get; }
        protected int StartDelay { get; set; } = 10;

        public IEnumerable<Process> Processes
        {
            get
            {
                lock (_processes)
                {
                    return _processes.ToList();
                }
            }
        }

        public IEnumerable<UsedPort> Ports
        {
            get
            {
                lock (_ports)
                {
                    return _ports.ToList();
                }
            }
        }
        public Process MainProcess => Processes.FirstOrDefault();
        public SolutionStatus Status { get; protected set; }
        public int ExpectedProcessCount { get; protected set; }

        public string Hint
        {
            get
            {
                var result = "";
                if (Processes.Any())
                {
                    result += "Processes:" + Environment.NewLine + "    ";
                    try
                    {
                        result += string.Join(Environment.NewLine + "    ",
                            Processes.Select(p => $"{p.Id} {(p.HasExited ? "" : p.ProcessName)}"));
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"Exception: {ex.Message}");
                    }

                    if (Ports.Any())
                    {
                        result += Environment.NewLine + "Ports:" + Environment.NewLine + "    ";
                        result += string.Join(Environment.NewLine + "    ", Ports);
                    }
                }
                else
                {
                    result = "Not running";
                }
                return result;
            }
        }

        public event EventHandler<EventArgs> Update;

        public string ProjectPath { get; protected set; }
        private readonly List<Process> _processes = new List<Process>();
        private readonly List<UsedPort> _ports = new List<UsedPort>();
        protected readonly Config.Config Config;
        protected readonly SolutionConfig SolutionConfig;

        private static readonly ManagementEventWatcher StartListener;
        private static readonly ManagementEventWatcher StopListener;
        private static readonly List<ProcessInfo> AllProcesses = new List<ProcessInfo>();
        protected class ProcessInfo
        {
            public ProcessInfo(ManagementBaseObject obj)
            {
                ProcessId = (int)(uint)obj["ProcessId"];
                ParentProcessId = (int)(uint)obj["ParentProcessId"];
                CommandLine = (string)obj["CommandLine"];
                Name = (string)obj["Name"];
            }

            public int ProcessId { get; }
            public int ParentProcessId { get; }
            public string CommandLine { get; }
            public string Name { get; }
            private Process _process;

            public Process Process => _process ??= Process.GetProcessById(ProcessId);
            public string WorkingDirectory => ProcessUtilities.GetCurrentDirectory(ProcessId);
        }

        static Solution()
        {
            var startQuery = new WqlEventQuery(
                "__InstanceCreationEvent",
                TimeSpan.FromMilliseconds(1000),
                "TargetInstance isa \"Win32_Process\"");

            var stopQuery = new WqlEventQuery(
                "__InstanceDeletionEvent",
                TimeSpan.FromMilliseconds(1000),
                "TargetInstance isa \"Win32_Process\"");

            StartListener = new ManagementEventWatcher(startQuery);
            StopListener = new ManagementEventWatcher(stopQuery);


            var existingQuery = new ObjectQuery("select ProcessId, ParentProcessId, CommandLine, Name from Win32_Process");
            var existingSearcher = new ManagementObjectSearcher(existingQuery);
            foreach (var obj in existingSearcher.Get())
            {
                lock (AllProcesses)
                {
                    AllProcesses.Add(new ProcessInfo(obj));
                }
            }
            StartListener.EventArrived += (p, e) =>
            {
                lock (AllProcesses)
                {
                    AllProcesses.Add(new ProcessInfo((ManagementBaseObject)e.NewEvent["TargetInstance"]));
                }
            };
            StopListener.EventArrived += (p, e) =>
            {
                lock (AllProcesses)
                {
                    AllProcesses.RemoveAll(x =>
                        x.ProcessId == new ProcessInfo((ManagementBaseObject)e.NewEvent["TargetInstance"]).ProcessId);
                }
            };

            StartListener.Start();
            StopListener.Start();

            var updateThread = new Thread(UpdateFunction);
            updateThread.Start();
            Application.ApplicationExit += (o, e) => _exited = true;
        }

        private static bool _exited = false;
        private static void UpdateFunction()
        {
            while (!_exited)
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
                foreach (var solution in AllSolutions)
                {
                    solution.DoUpdate();
                }
            }
        }


        protected Solution(string solutionPath, string projectPath, Dictionary<string, LaunchProfile> profiles, Config.Config config)
        {
            SolutionPath = solutionPath;
            ProjectPath = projectPath;
            Profiles = profiles;
            Config = config;
            // ReSharper disable once VirtualMemberCallInConstructor
            SetName();
            SolutionConfig = Config.GetSolution(Name);
            Hidden = SolutionConfig.Hidden;
            Checked = SolutionConfig.Checked && !Hidden;

            GetExistingProcesses();
            StartListener.EventArrived += StartListenerOnEventArrived;
            AllSolutions.Add(this);
        }

        private void StartListenerOnEventArrived(object sender, EventArrivedEventArgs e)
        {
            NewProcess(new ProcessInfo((ManagementBaseObject)e.NewEvent["TargetInstance"]));
        }

        private void GetExistingProcesses()
        {
            lock (AllProcesses)
            {
                foreach (var processInfo in AllProcesses)
                {
                    NewProcess(processInfo);
                }
            }
        }

        protected enum ProcessKind
        {
            Irrelevant,
            Relevant,
            Main
        }
        protected abstract ProcessKind IsRelevantProcess(ProcessInfo processInfo);

        private static bool IsFrameworkProcess(ProcessInfo processInfo)
        {
            if (processInfo.Name == "conhost.exe") return true;
            if (processInfo.Name == "dotnet.exe" && processInfo.CommandLine.Contains("\\MSBuild.dll\"")) return true;
            return false;
        }

        protected void NewProcess(ProcessInfo processInfo)
        {
            try
            {
                var relevant = IsRelevantProcess(processInfo);
                if (relevant == ProcessKind.Main)
                {
                    AddMainProcess(processInfo.Process);
                }
                else if (relevant == ProcessKind.Relevant)
                {
                    AddProcess(processInfo.Process);
                }
                else if (Processes.Any(a => a.Id == processInfo.ParentProcessId) && !IsFrameworkProcess(processInfo))
                {
                    AddProcess(processInfo.Process);
                }
                else
                {
                    return;
                }
                DoUpdate();
            }
            catch (Exception)
            {
                //
            }
        }
        protected void AddProcess(Process process)
        {
            if (Processes.Any(a => a.Id == process.Id)) return;

            try
            {
                process.EnableRaisingEvents = true;
                process.Exited += RemoveProcess;
            }
            catch
            {
                return;
            }

            lock (_processes)
            {
                _processes.Add(process);
            }

            DoUpdate();
        }

        protected void AddMainProcess(Process process)
        {
            if (Processes.Any(a => a.Id == process.Id)) return;

            lock (_processes)
            {
                _processes.Insert(0, process);
            }
            process.EnableRaisingEvents = true;
            process.Exited += RemoveProcess;
            DoUpdate();
        }

        private void RemoveProcess(object sender, EventArgs e)
        {
            var process = (Process)sender;
            if (!process.HasExited) return;

            if (process == MainProcess)
            {
                RedirectedOutput = false;
            }

            lock (_processes)
            {
                _processes.Remove(process);
            }
            DoUpdate();
        }

        protected void StartProcess(string filename, string arguments)
        {
            var newProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    WorkingDirectory = ProjectPath,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            newProcess.OutputDataReceived += (s, e1) => OnOutput(e1.Data);
            newProcess.ErrorDataReceived += (s, e1) => OnError(e1.Data);
            try
            {
                newProcess.Start();
                newProcess.BeginErrorReadLine();
                newProcess.BeginOutputReadLine();
                AddMainProcess(newProcess);
                Status = SolutionStatus.Partial;
                RedirectedOutput = true;
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
                Status = SolutionStatus.Stopped;
            }
            DoUpdate();
        }

        public void Start(string profile)
        {
            if (Processes.Any(p => !p.HasExited)) return;
            _outputBuilder.Clear();
            _errorBuilder.Clear();
            _richText.Clear();
            _richText.Clear();
            DoStart(profile);
        }

        protected abstract void DoStart(string profile);

        public void Stop()
        {
            foreach (var process in Processes)
            {
                process.Kill();
            }
            DoUpdate();
        }

        public void Restart(string profile)
        {
            Stop();
            Start(profile);
        }

        public void AttachDebugger()
        {
            MainProcess.AttachDebugger(SolutionPath);
            DoUpdate();
        }

        protected void DoUpdate()
        {
            Status = !Processes.Any() ? SolutionStatus.Stopped :
                Processes.Any(a => !a.Responding) ? SolutionStatus.Unresponsive :
                Processes.Any(a => a.HasDebuggerAttached()) ? SolutionStatus.Debugged :
                Processes.Count() < ExpectedProcessCount ? SolutionStatus.Partial :
                SolutionStatus.Running;
            UpdatePorts();
            Update?.Invoke(this, EventArgs.Empty);
        }


        private readonly AnsiText _richText = new AnsiText();
        private readonly AnsiText _fullRichText = new AnsiText();
        public string RichText => RedirectedOutput && _outputBuilder.Length != 0 ? _richText.ToString() : "{\\rtf1\\ansi\\pard\n[No output redirected for this process]\\par\n}";
        private readonly StringBuilder _outputBuilder = new StringBuilder();
        private readonly StringBuilder _errorBuilder = new StringBuilder();
        public event EventHandler<TextEventArgs> OutputAdded;
        public event EventHandler<TextEventArgs> ErrorAdded;
        public string Output => _outputBuilder.ToString();
        public string Error => _errorBuilder.ToString();
        public abstract Image Image { get; }

        protected void OnOutput(string outputDelta)
        {
            if (outputDelta == null) return;

            _richText.Newline();
            _richText.Append(outputDelta);
            _fullRichText.Newline();
            _fullRichText.Append(outputDelta);
            _outputBuilder.AppendLine(outputDelta);
            OutputAdded?.Invoke(this, new TextEventArgs(Output, outputDelta));
        }

        protected void OnError(string errorDelta)
        {
            if (errorDelta == null) return;

            _richText.Newline();
            _richText.Red();
            _richText.Append(errorDelta);
            _richText.Black();
            _fullRichText.Newline();
            _fullRichText.Red();
            _fullRichText.Append(errorDelta);
            _fullRichText.Black();
            _errorBuilder.AppendLine(errorDelta);
            ErrorAdded?.Invoke(this, new TextEventArgs(Error, errorDelta));
        }

        protected void UpdatePorts()
        {
            var pids = Processes.Select(a => a.Id).ToList();
            lock (_ports)
            {
                var usage = ProcessUtilities.GetPortUsage();
                var relevant = usage.Where(a => pids.Contains(a.ProcessId)).ToList();
                _ports.Clear();
                _ports.AddRange(relevant);
                Trace.WriteLine($"{Name}: {pids.Count} pids, {relevant.Count} port usages");
            }
        }

        public static IEnumerable<Solution> GetSolutions(Config.Config config)
        {
            foreach (var solution in DotnetSolution.GetDotnetSolutions(config))
            {
                yield return solution;
            }

            foreach (var solution in YarnSolution.GetYarnSolutions(config))
            {
                yield return solution;
            }
        }

        protected abstract void SetName();

        public void ClearOutput()
        {
            _richText.Clear();
            Update?.Invoke(this, EventArgs.Empty);
        }

        public void RestoreOutput()
        {
            _fullRichText.CopyTo(_richText);
            Update?.Invoke(this, EventArgs.Empty);
        }

        public abstract void Open();
    }
}