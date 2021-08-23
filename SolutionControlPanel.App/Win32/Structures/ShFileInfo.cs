using System;
using System.Runtime.InteropServices;

namespace SolutionControlPanel.App.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShFileInfo
    {
        public const int NameSize = 80;
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NameSize)]
        public string szTypeName;
    }
}
