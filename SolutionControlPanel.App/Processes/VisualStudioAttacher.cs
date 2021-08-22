using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using EnvDTE;
using DTEProcess = EnvDTE.Process;
using Process = System.Diagnostics.Process;

namespace SolutionControlPanel.App.Processes
{
    internal static class VisualStudioAttacher
    {
        [DllImport("ole32.dll")]
        public static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("ole32.dll")]
        public static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// The method to use to attach visual studio to a specified process.
        /// </summary>
        /// <param name="visualStudioProcess">
        /// The visual studio process to attach to.
        /// </param>
        /// <param name="applicationProcess">
        /// The application process that needs to be debugged.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the application process is null.
        /// </exception>
        public static void AttachVisualStudioToProcess(Process visualStudioProcess, Process applicationProcess)
        {
            if (!TryGetVsInstance(visualStudioProcess.Id, out var visualStudioInstance)) return;


            // Find the process you want the VS instance to attach to...
            var processToAttachTo = visualStudioInstance.Debugger.LocalProcesses.Cast<DTEProcess>()
                    .FirstOrDefault(process => process.ProcessID == applicationProcess.Id);

            // Attach to the process.
            if (processToAttachTo != null)
            {
                processToAttachTo.Attach();

                ShowWindow((int)visualStudioProcess.MainWindowHandle, 3);
                SetForegroundWindow(visualStudioProcess.MainWindowHandle);
            }
            else
            {
                throw new InvalidOperationException("Visual Studio process cannot find specified application '" + applicationProcess.Id + "'");
            }
        }

        /// <summary>
        /// The get visual studio process that is running and has the specified solution loaded.
        /// </summary>
        /// <param name="solutionName">
        /// The solution name to look for.
        /// </param>
        /// <returns>
        /// The visual studio <see cref="Process"/> with the specified solution name.
        /// </returns>
        private static Process GetVisualStudioForSolution(string solutionName)
        {
            var visualStudios = GetVisualStudioProcesses();

            foreach (var visualStudio in visualStudios)
            {
                if (!TryGetVsInstance(visualStudio.Id, out var visualStudioInstance)) continue;

                var actualSolutionName = Path.GetFileName(visualStudioInstance.Solution.FullName);

                if (string.Compare(
                    actualSolutionName,
                    solutionName,
                    StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return visualStudio;
                }
            }

            return null;
        }

        [DllImport("User32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);

        private static IEnumerable<Process> GetVisualStudioProcesses()
        {
            var processes = Process.GetProcesses();
            return processes.Where(o => o.ProcessName.Contains("devenv"));
        }

        private static bool TryGetVsInstance(int processId, out _DTE instance)
        {
            var numFetched = IntPtr.Zero;
            var monikers = new IMoniker[1];

            GetRunningObjectTable(0, out var runningObjectTable);
            runningObjectTable.EnumRunning(out var monikerEnumerator);
            monikerEnumerator.Reset();

            while (monikerEnumerator.Next(1, monikers, numFetched) == 0)
            {
                CreateBindCtx(0, out var ctx);

                monikers[0].GetDisplayName(ctx, null, out var runningObjectName);

                runningObjectTable.GetObject(monikers[0], out var runningObjectVal);

                if (!(runningObjectVal is _DTE dte) || !runningObjectName.StartsWith("!VisualStudio")) continue;


                var currentProcessId = int.Parse(runningObjectName.Split(':')[1]);

                if (currentProcessId != processId) continue;


                instance = dte;
                return true;
            }

            instance = null;
            return false;
        }

        public static void AttachDebugger(this Process process, string solutionName)
        {
            var vsProcess = GetVisualStudioForSolution(solutionName);
            if (vsProcess == null || !TryGetVsInstance(vsProcess.Id, out var vsInstance))
            {
                var version = 19;
                Type type = null;
                while (type == null && version >= 15)
                {
                    type = Type.GetTypeFromProgID($"VisualStudio.DTE.{version}.0");
                    --version;
                }

                if (type == null)
                {
                    MessageBox.Show(@"Visual Studio not found. Are you sure it's installed?", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                vsInstance = (DTE)Activator.CreateInstance(type, true);
                if (vsInstance == null)
                {

                    MessageBox.Show(@"Couldn't instantiate Visual Studio", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                vsInstance.Solution.Open(solutionName);
            }

            var p = vsInstance.Debugger.LocalProcesses.OfType<EnvDTE.Process>().FirstOrDefault(a =>
            {

                try
                {
                    return a.ProcessID == process.Id;
                }
                catch
                {
                    return false;
                }
            });
            p.Attach();
            vsInstance.MainWindow.Visible = true;
        }
    }
}
