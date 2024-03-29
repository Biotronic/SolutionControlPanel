﻿using System.Collections.Generic;

// ReSharper disable UnusedMember.Global

namespace SolutionControlPanel.App.Processes
{
    public class LaunchSettings
    {
        public LaunchIisSettings IisSettings { get; set; }
        public Dictionary<string, LaunchProfile> Profiles { get; set; }
    }
}