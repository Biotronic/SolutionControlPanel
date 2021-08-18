using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using IoPath = System.IO.Path;

namespace Volmax.ControlPanel.App.Processes
{
    internal class DotnetSolution : Solution
    {
        public DotnetSolution(string path, string projectPath, Dictionary<string, LaunchProfile> profiles,
            Config.Config config) : base(path, projectPath, profiles, config)
        {
            ExpectedProcessCount = 2;
            Profile = SolutionConfig.Profile;
        }

        protected override void SetName()
        {
            Name = IoPath.GetFileNameWithoutExtension(Path);
        }

        protected override ProcessKind IsRelevantProcess(ProcessInfo processInfo)
        {
            if (processInfo.Name != "dotnet.exe") return ProcessKind.Irrelevant;

            if (!Regex.IsMatch(processInfo.CommandLine, $"^\"dotnet\" run -p {Name}\\.csproj")) return ProcessKind.Irrelevant;

            AddMainProcess(processInfo.Process);

            var findProfile = Regex.Match(processInfo.CommandLine, "--launch-profile (\\S+)");
            Profile = findProfile.Success ? findProfile.Groups[1].Value : "(unknown)";
            return ProcessKind.Main;
        }

        protected override void DoStart(string profile)
        {
            StartProcess("dotnet", $"run -p {Name}.csproj --launch-profile {profile}");
        }

        public static IEnumerable<Solution> GetDotnetSolutions(Config.Config config)
        {
            var solutions = Directory.EnumerateFiles(config.Basepath, "*.sln", SearchOption.AllDirectories);

            foreach (var solution in solutions)
            {
                var solutionName = IoPath.GetFileNameWithoutExtension(solution);

                var solutionDir = IoPath.GetDirectoryName(solution);
                if (string.IsNullOrWhiteSpace(solutionDir)) continue;

                var projectPath = IoPath.Combine(solutionDir, "src", solutionName);

                var launchSettings = IoPath.Combine(projectPath, @"Properties\launchSettings.json");
                if (!File.Exists(launchSettings)) continue;

                var settings = JsonConvert.DeserializeObject<LaunchSettings>(File.ReadAllText(launchSettings));
                if (settings == null) continue;

                if (settings.Profiles.Any(a => !string.IsNullOrWhiteSpace(a.Value.CommandLineArgs))) continue;

                yield return new DotnetSolution(solution, projectPath, settings.Profiles, config);
            }
        }
    }
}