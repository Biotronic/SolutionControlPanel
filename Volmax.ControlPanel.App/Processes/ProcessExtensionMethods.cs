using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

namespace Volmax.ControlPanel.App.Processes
{
    public static class ProcessExtensionMethods
    {
        public static IEnumerable<Process> GetChildProcesses(this Process process)
        {
            var children = new List<Process>();
            var mos = new ManagementObjectSearcher($"Select * From Win32_Process Where ParentProcessID={process.Id}");

            foreach (var mo in mos.Get())
            {
                children.Add(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])));
            }

            return children;
        }

        [DllImport("Kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CheckRemoteDebuggerPresent(SafeHandle hProcess, [MarshalAs(UnmanagedType.Bool)] ref bool isDebuggerPresent);

        public static bool HasDebuggerAttached(this Process process)
        {
            var attached = false;
            if (CheckRemoteDebuggerPresent(process.SafeHandle, ref attached))
            {
                return attached;
            }
            return false;
        }
    }
}
