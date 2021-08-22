using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using IoPath = System.IO.Path;

namespace SolutionControlPanel.App.Processes
{
    internal class DotnetSolution : Solution
    {
        public DotnetSolution(string solutionPath, string projectPath, Dictionary<string, LaunchProfile> profiles,
            Config.Config config) : base(solutionPath, projectPath, profiles, config)
        {
            ExpectedProcessCount = 2;
            Profile = SolutionConfig.Profile;
        }

        public override Image Image => GetFileIcon(".sln");

        protected override void SetName()
        {
            Name = IoPath.GetFileName(ProjectPath);
        }

        public override void Open()
        {
            using var fileopener = new Process
            {
                StartInfo =
                {
                    FileName = SolutionPath
                }
            };
            fileopener.Start();
        }

        public override void OpenInBrowser()
        {
            var port = Ports.GroupBy(a => a.ProcessId).SingleOrDefault(a => a.Count() > 1)?.First().Port;
            if (!port.HasValue)
            {
                MessageBox.Show(@"No unambiguous port number found.", @"Failed to open", MessageBoxButtons.OK);
                return;
            }

            var url = $"http://localhost:{port}/swagger/index.html";
            Process.Start("explorer.exe", $"\"{url}\"");
        }

        protected override ProcessKind IsRelevantProcess(ProcessInfo processInfo)
        {
            if (processInfo.Name != "dotnet.exe")
            {
                if (processInfo.CommandLine?.ToLowerInvariant().Contains(ProjectPath.ToLowerInvariant()) ?? false) return ProcessKind.Relevant;
                return ProcessKind.Irrelevant;
            }

            if (!Regex.IsMatch(processInfo.CommandLine, $"^\"dotnet\" run -p {Name}\\.csproj")) return ProcessKind.Irrelevant;

            AddMainProcess(processInfo.Process);

            var findProfile = Regex.Match(processInfo.CommandLine, "--launch-profile (\\S+)");
            Profile = findProfile.Success ? findProfile.Groups[1].Value : "(unknown)";
            return ProcessKind.Main;
        }

        protected override void DoStart(string profile)
        {
            StartProcess("dotnet", $"run -p {IoPath.GetFileNameWithoutExtension(SolutionPath)}.csproj --launch-profile {profile}");
        }

        private static Image GetFileIcon(string name)
        {
            var shfi = new SHFILEINFO();
            SHGetFileInfo(name, 0x80, ref shfi, (uint)Marshal.SizeOf(shfi), 0x111);
            var icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone();
            DestroyIcon(shfi.hIcon);
            return icon.ToBitmap();
        }

        [DllImport("Shell32.dll")]
        public static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags
        );

        [DllImport("User32.dll")]
        public static extern int DestroyIcon(IntPtr hIcon);
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public const int NAMESIZE = 80;
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NAMESIZE)]
            public string szTypeName;
        };

        public static IEnumerable<Solution> GetDotnetSolutions(Config.Config config)
        {
            var solutions = Directory.EnumerateFiles(config.Basepath, "*.sln", SearchOption.AllDirectories);

            foreach (var solution in solutions)
            {
                var solutionDir = IoPath.GetDirectoryName(solution);
                if (string.IsNullOrWhiteSpace(solutionDir)) continue;

                var projects = Directory.EnumerateFiles(solutionDir, "*.csproj", SearchOption.AllDirectories).ToList();

                foreach (var project in projects)
                {
                    var projectDir = IoPath.GetDirectoryName(project) ?? "";
                    var launchSettings = IoPath.Combine(projectDir, @"Properties\launchSettings.json");
                    if (!File.Exists(launchSettings)) continue;

                    var settings = JsonConvert.DeserializeObject<LaunchSettings>(File.ReadAllText(launchSettings));
                    if (settings == null) continue;

                    if (settings.Profiles.Any(a => !string.IsNullOrWhiteSpace(a.Value.CommandLineArgs))) continue;

                    yield return new DotnetSolution(solution, projectDir, settings.Profiles, config);
                }
            }
        }
    }
}