using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Volmax.ControlPanel.App.Config
{
    internal class Config
    {
        private readonly string _source;
        public Config(string source)
        {
            _source = source;

            if (!File.Exists(_source))
            {
                var dlg = new FolderBrowserDialog
                {
                    Description = "Please select the base folder containing your projects",
                    UseDescriptionForTitle = true
                };
                var r = dlg.ShowDialog();
                if (r == DialogResult.Cancel)
                {
                    Application.Exit();
                    return;
                }
                var basePath = dlg.SelectedPath;
                File.WriteAllText(_source, JsonConvert.SerializeObject(new { Basepath = basePath }));
            }

            JsonConvert.PopulateObject(File.ReadAllText(source), this);
            foreach (var (_, value) in Solutions)
            {
                value.Config = this;
            }
        }

        public string Basepath { get; set; }
        public Dictionary<string, SolutionConfig> Solutions { get; set; } = new Dictionary<string, SolutionConfig>();

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
    }
}