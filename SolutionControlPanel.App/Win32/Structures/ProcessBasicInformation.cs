using System;
using System.Runtime.InteropServices;

namespace SolutionControlPanel.App.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ProcessBasicInformation
    {
        public readonly IntPtr Reserved1;
        public readonly IntPtr PebBaseAddress;
        public readonly IntPtr Reserved2_0;
        public readonly IntPtr Reserved2_1;
        public readonly IntPtr UniqueProcessId;
        public readonly IntPtr Reserved3;
    }
}