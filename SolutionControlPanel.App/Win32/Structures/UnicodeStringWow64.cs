using System.Runtime.InteropServices;

namespace SolutionControlPanel.App.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct UnicodeStringWow64
    {
        public readonly short Length;
        public readonly short MaximumLength;
        public readonly long Buffer;
    }
}