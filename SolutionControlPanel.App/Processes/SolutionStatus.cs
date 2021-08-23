namespace SolutionControlPanel.App.Processes
{
    public enum SolutionStatus
    {
        [Mark("Stopped", "Offline_16x")]
        Stopped,
        [Mark("Running", "OK_16x")]
        Running,
        [Mark("Unresponsive", "Warning_16x")]
        Unresponsive,
        [Mark("Partial", "Alert_16x")]
        Partial,
        [Mark("Debugged", "Debug_16x")]
        Debugged
    }
}