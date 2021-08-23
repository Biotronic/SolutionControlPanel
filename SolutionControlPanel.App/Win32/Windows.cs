using System.Drawing;
using System.Runtime.InteropServices;
using SolutionControlPanel.App.Win32.Structures;

namespace SolutionControlPanel.App.Win32
{
    public static class Windows
    {
        public static Image GetIconForExtension(string name)
        {
            var shfi = new ShFileInfo();
            Shell32.SHGetFileInfo(name, 0x80, ref shfi, (uint)Marshal.SizeOf(shfi), 0x111);
            var icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone();
            User32.DestroyIcon(shfi.hIcon);
            return icon.ToBitmap();
        }
    }
}