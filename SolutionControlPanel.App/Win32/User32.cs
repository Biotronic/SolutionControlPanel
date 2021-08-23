using System;
using System.Runtime.InteropServices;

namespace SolutionControlPanel.App.Win32
{
    public static class User32
    {
        [DllImport("User32.dll")]
        public static extern int DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32")]
        public static extern int ShowWindow(int hwnd, int nCmdShow);
    }
}