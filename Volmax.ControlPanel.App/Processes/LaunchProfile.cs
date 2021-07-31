using System.Collections.Generic;
// ReSharper disable UnusedMember.Global

namespace Volmax.ControlPanel.App.Processes
{
    public class LaunchProfile
    {
        public string CommandName { get; set; }
        public string CommandLineArgs { get; set; }
        public bool LaunchBrowser { get; set; }
        public string LaunchUrl { get; set; }
        public string ApplicationUrl { get; set; }
        public Dictionary<string, string> EnvironmentVariables { get; set; }
    }
}