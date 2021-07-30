using System.Collections.Generic;

namespace Volmax.ControlPanel.App.Processes
{
    public class LaunchSettings
    {
        public LaunchIisSettings IisSettings { get; set; }
        public Dictionary<string, LaunchProfile> Profiles { get; set; }
    }
}