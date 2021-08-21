﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Volmax.ControlPanel.App.Processes
{
    internal class YarnSolution : Solution
    {
        public YarnSolution(string solutionPath, string projectPath, Dictionary<string, LaunchProfile> profiles, Config.Config config)
            : base(solutionPath, projectPath, profiles, config)
        {
            ExpectedProcessCount = 8;
        }

        public override Image Image => Properties.Resources.Yarn_16x;

        protected override void SetName()
        {
            Name = Path.GetRelativePath(Config.Basepath, SolutionPath).Split('\\', '/').First();
        }

        public override void Open()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "Programs\\Microsoft VS Code\\bin\\code.cmd");

            using var codeProcess = new Process
            {
                StartInfo =
                {
                    FileName = path,
                    ArgumentList = { SolutionPath },
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            codeProcess.Start();
        }

        protected override ProcessKind IsRelevantProcess(ProcessInfo processInfo)
        {
            if (processInfo.CommandLine == null) return ProcessKind.Irrelevant;
            if (Regex.IsMatch(processInfo.CommandLine, "\"cmd\" /c yarn start"))
            {
                if (Path.GetRelativePath(ProjectPath, processInfo.WorkingDirectory) == ".")
                {
                    return ProcessKind.Main;
                }
            }
            else if (Regex.IsMatch(processInfo.CommandLine, $"^.*node.exe\"\\s+\"{Regex.Escape(ProjectPath)}"))
            {
                return ProcessKind.Relevant;
            }
            return ProcessKind.Irrelevant;
        }

        protected override void DoStart(string profile)
        {
            StartProcess("cmd", "/c yarn start");
        }

        public static IEnumerable<Solution> GetYarnSolutions(Config.Config config)
        {
            var paths = Directory.EnumerateFiles(config.Basepath, ".npmrc", SearchOption.AllDirectories);

            foreach (var path in paths)
            {
                yield return new YarnSolution(Path.GetDirectoryName(path),
                    Path.GetDirectoryName(path), new Dictionary<string, LaunchProfile>(), config);
            }
        }
    }
}