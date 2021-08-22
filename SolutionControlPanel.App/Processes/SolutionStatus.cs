namespace SolutionControlPanel.App.Processes
{
    public enum SolutionStatus
    {
        [Mark("Stopped", "StatusOffline_16x")]
        Stopped,
        [Mark("Running", "StatusOK_16x")]
        Running,
        [Mark("Unresponsive", "StatusWarning_16x")]
        Unresponsive,
        [Mark("Partial", "StatusAlert_16x")]
        Partial,
        [Mark("Debugged", "StatusDebug_16x")]
        Debugged
    }
}