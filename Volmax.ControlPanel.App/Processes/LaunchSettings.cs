using System.Collections.Generic;
// ReSharper disable UnusedMember.Global

namespace Volmax.ControlPanel.App.Processes
{
    public class LaunchSettings
    {
        public LaunchIisSettings IisSettings { get; set; }
        public Dictionary<string, LaunchProfile> Profiles { get; set; }
    }
}