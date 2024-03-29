﻿using System;

namespace SolutionControlPanel.App.Utils
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
