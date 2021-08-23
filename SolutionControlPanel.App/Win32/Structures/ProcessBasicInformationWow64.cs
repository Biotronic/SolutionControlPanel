using System.Runtime.InteropServices;

namespace SolutionControlPanel.App.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ProcessBasicInformationWow64
    {
        public readonly long Reserved1;
        public readonly long PebBaseAddress;
        public readonly long Reserved2_0;
        public readonly long Reserved2_1;
        public readonly long UniqueProcessId;
        public readonly long Reserved3;
    }
}