using System;
using System.Runtime.InteropServices;
using SolutionControlPanel.App.Win32.Structures;

namespace SolutionControlPanel.App.Win32
{
    public static class Shell32
    {
        [DllImport("Shell32.dll")]
        public static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref ShFileInfo psfi,
            uint cbFileInfo,
            uint uFlags
        );
    }
}
