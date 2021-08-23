using System;
using System.Runtime.InteropServices;

namespace SolutionControlPanel.App.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct UnicodeString
    {
        public readonly short Length;
        public readonly short MaximumLength;
        public readonly IntPtr Buffer;
    }
}