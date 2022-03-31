using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using SolutionControlPanel.App.Win32;

namespace SolutionControlPanel.App.Processes
{
    internal class DotnetSolution : Solution
    {
        public DotnetSolution(string solutionPath, string projectPath, Dictionary<string, LaunchProfile> profiles,
            Config.Config config) : base(solutionPath, projectPath, profiles, config)
        {
            ExpectedProcessCount = 2;
            Profile = SolutionConfig.Profile;
        }

        public override Image Image => Windows.GetIconForExtension(".sln");

        protected override void SetName()
        {
            Name = Path.GetFileName(ProjectPath);
        }

        public override void Open()
        {
            using var fileopener = new Process
            {
                StartInfo =
                {
                    FileName = SolutionPath
                }
            };
            fileopener.Start();
        }

        public override void OpenInBrowser()
        {
            var port = Ports.GroupBy(a => a.ProcessId).SingleOrDefault(a => a.Count() > 1)?.First().Port;
            if (!port.HasValue)
            {
                MessageBox.Show(@"No unambiguous port number found.", @"Failed to open", MessageBoxButtons.OK);
                return;
            }

            var url = $"http://localhost:{port}/swagger/index.html";
            Process.Start("explorer.exe", $"\"{url}\"");
        }

        protected override ProcessKind IsRelevantProcess(ProcessInfo processInfo)
        {
            if (processInfo.Name != "dotnet.exe")
            {
                if (processInfo.CommandLine?.ToLowerInvariant().Contains(ProjectPath.ToLowerInvariant()) ?? false) return ProcessKind.Relevant;
                return ProcessKind.Irrelevant;
            }

            if (!Regex.IsMatch(processInfo.CommandLine, $"^\"dotnet\" run -p {Name}\\.csproj") && !Regex.IsMatch(processInfo.CommandLine, $"^\"dotnet\" run --project {Name}\\.csproj")) return ProcessKind.Irrelevant;

            AddMainProcess(processInfo.Process);

            var findProfile = Regex.Match(processInfo.CommandLine, "--launch-profile (\\S+)");
            Profile = findProfile.Success ? findProfile.Groups[1].Value : "(unknown)";
            return ProcessKind.Main;
        }

        protected override void DoStart(string profile)
        {
            StartProcess("dotnet", $"run --project {Path.GetFileNameWithoutExtension(SolutionPath)}.csproj --launch-profile {profile}");
        }

        public static IEnumerable<Solution> GetDotnetSolutions(Config.Config config)
        {
            var solutions = Directory.EnumerateFiles(config.Basepath, "*.sln", SearchOption.AllDirectories).ToList();

            foreach (var solution in solutions)
            {
                var solutionDir = Path.GetDirectoryName(solution);
                if (string.IsNullOrWhiteSpace(solutionDir)) continue;

                var projects = Directory.EnumerateFiles(solutionDir, "*.csproj", SearchOption.AllDirectories).ToList();

                foreach (var project in projects)
                {
                    var projectDir = Path.GetDirectoryName(project) ?? "";
                    var launchSettings = Path.Combine(projectDir, @"Properties\launchSettings.json");
                    if (!File.Exists(launchSettings)) continue;

                    var settings = JsonConvert.DeserializeObject<LaunchSettings>(File.ReadAllText(launchSettings));
                    if (settings == null) continue;

                    if (settings.Profiles.Any(a => !string.IsNullOrWhiteSpace(a.Value.CommandLineArgs))) continue;

                    yield return new DotnetSolution(solution, projectDir, settings.Profiles, config);
                }
            }
        }
    }
}