using System.Runtime.InteropServices;

namespace SolutionControlPanel.App.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct UnicodeString32
    {
        public readonly short Length;
        public readonly short MaximumLength;
        public readonly int Buffer;
    }
}