using System;

namespace Volmax.ControlPanel.App
{
    public class TextEventArgs : EventArgs
    {
        public TextEventArgs(string allText, string delta)
        {
            AllText = allText;
            Delta = delta;
        }

        public string AllText { get; set; }
        public string Delta { get; set; }
    }
}
