using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Biotronic.SolutionControlPanel.App.Config
{
    internal class Config
    {
        private readonly string _source;
        public Config(string source)
        {
            _source = source;

            if (!File.Exists(_source))
            {
                if (!SetBasePath())
                {
                    Application.Exit();
                    return;
                }
            }

            JsonConvert.PopulateObject(File.ReadAllText(source), this);
            foreach (var (_, value) in Solutions)
            {
                value.Config = this;
            }
        }

        public string Basepath { get; set; }
        public Dictionary<string, SolutionConfig> Solutions { get; set; } = new Dictionary<string, SolutionConfig>();
        public bool StartProjectsAutomatically { get; set; }
        public string DefaultProfilePattern { get; set; } = "\\.DevDb$";

        public void Update()
        {
            File.WriteAllText(_source, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public SolutionConfig GetSolution(string name)
        {
            if (Solutions.TryGetValue(name, out var existing))
                return existing;
            return Solutions[name] = new SolutionConfig(this);
        }

        public bool SetBasePath()
        {
            var dlg = new FolderBrowserDialog
            {
                Description = @"Please select the base folder containing your projects",
                UseDescriptionForTitle = true
            };
            var r = dlg.ShowDialog();
            if (r == DialogResult.Cancel)
            {
                return false;
            }

            Solutions.Clear();
            Basepath = dlg.SelectedPath;
            Update();
            return true;
        }
    }
}