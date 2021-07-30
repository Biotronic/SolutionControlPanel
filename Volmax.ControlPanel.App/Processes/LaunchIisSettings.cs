namespace Volmax.ControlPanel.App.Processes
{
    public class LaunchIisSettings
    {
        public bool WindowsAuthentication { get; set; }
        public bool AnonymousAuthentication { get; set; }
        public LaunchIisSettingsIisExpress IisExpress { get; set; }
    }
}