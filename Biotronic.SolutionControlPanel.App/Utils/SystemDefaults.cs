using System;
using System.Drawing;
using Microsoft.Win32;

namespace Biotronic.SolutionControlPanel.App.Utils
{
    public static class SystemDefaults
    {

        public static Image DefaultBrowserIcon => IconFromFilePath(DefaultBrowser)?.ToBitmap();

        public static string DefaultBrowser
        {
            get
            {
                try
                {
                    const string path = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.htm\\UserChoice";
                    using var regDefault = Registry.CurrentUser.OpenSubKey(path, false);
                    var progId = regDefault?.GetValue("ProgId");

                    using var regKey = Registry.ClassesRoot.OpenSubKey(progId + "\\shell\\open\\command", false);
                    var name = regKey?.GetValue(null).ToString()?.ToLower().Replace("" + (char)34, "");

                    if (!name?.EndsWith("exe") ?? false)
                    {
                        name = name[..(name.LastIndexOf(".exe", StringComparison.Ordinal) + 4)];
                    }
                    return name;

                }
                catch
                {
                    return null;
                }
            }
        }

        private static Icon IconFromFilePath(string filePath)
        {
            try
            {
                return Icon.ExtractAssociatedIcon(filePath);
            }
            catch
            {
                return null;
            }
        }
    }
}
