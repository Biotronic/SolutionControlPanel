using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Biotronic.SolutionControlPanel.App.Processes
{
    public static class ProcessExtensionMethods
    {
        [DllImport("Kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CheckRemoteDebuggerPresent(SafeHandle hProcess, [MarshalAs(UnmanagedType.Bool)] ref bool isDebuggerPresent);

        public static bool HasDebuggerAttached(this Process process)
        {
            var attached = false;
            try
            {
                if (CheckRemoteDebuggerPresent(process.SafeHandle, ref attached))
                {
                    return attached;
                }
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return false;
        }
    }
}
