using System;
using System.IO;
using System.Windows.Forms;
using Biotronic.SolutionControlPanel.App.Properties;
using IWshRuntimeLibrary;
using File = System.IO.File;

namespace Biotronic.SolutionControlPanel.App.Utils
{
    public static class RunAtStartup
    {
        private static string StartupLinkPath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Startup),
            $"{Resources.AppName}.lnk");

        public static bool Registered => File.Exists(StartupLinkPath);

        public static void Register()
        {
            var wshShell = new WshShell();
            var shortcut = (IWshShortcut)wshShell.CreateShortcut(StartupLinkPath);

            var exePath = Path.ChangeExtension(Application.ExecutablePath, "exe");
            shortcut.TargetPath = exePath;
            shortcut.Arguments = Resources.AtStartup;
            shortcut.WorkingDirectory = Application.StartupPath;
            shortcut.Description = Resources.AppName;
            shortcut.IconLocation = exePath + ",0";
            shortcut.Save();
        }

        public static void Unregister()
        {
            File.Delete(StartupLinkPath);
        }
    }
}
