using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Volmax.ControlPanel.App.Processes
{
    internal class YarnSolution : Solution
    {
        public YarnSolution(string path, string projectPath, Dictionary<string, LaunchProfile> profiles, Config.Config config)
            : base(path, projectPath, profiles, config)
        {
            ExpectedProcessCount = 8;
        }

        protected override void SetName()
        {
            Name = System.IO.Path.GetRelativePath(Config.Basepath, Path).Split('\\', '/').First();
        }

        protected override ProcessKind IsRelevantProcess(ProcessInfo processInfo)
        {
            if (Regex.IsMatch(processInfo.CommandLine, "\"cmd\" /c yarn start"))
            {
                if (System.IO.Path.GetRelativePath(ProjectPath, processInfo.WorkingDirectory) == ".")
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
                yield return new YarnSolution(System.IO.Path.GetDirectoryName(path),
                    System.IO.Path.GetDirectoryName(path), new Dictionary<string, LaunchProfile>(), config);
            }
        }
    }
}