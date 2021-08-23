using System;
using System.Diagnostics;
using SolutionControlPanel.App.Win32;

namespace SolutionControlPanel.App.Processes
{
    public static class ProcessExtensionMethods
    {
        public static bool HasDebuggerAttached(this Process process)
        {
            var attached = false;
            try
            {
                if (Kernel32.CheckRemoteDebuggerPresent(process.SafeHandle, ref attached))
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
