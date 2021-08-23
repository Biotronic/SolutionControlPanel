using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SolutionControlPanel.App.Config
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
            if (!Directory.Exists(Basepath))
            {
                MessageBox.Show($"The path {Basepath} does not appear to exists. Please choose a different path to your solutions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (!SetBasePath())
                {
                    Application.Exit();
                    return;
                }
                JsonConvert.PopulateObject(File.ReadAllText(source), this);
            }
            foreach (var (_, value) in Solutions)
            {
                value.Config = this;
            }
        }

        public string Basepath { get; set; }
        public Dictionary<string, SolutionConfig> Solutions { get; set; } = new Dictionary<string, SolutionConfig>();
        public bool StartProjectsAutomatically { get; set; }
        public string DefaultProfilePattern { get; set; } = "\\.DevDb$";
        public MainFormConfig MainForm { get; set; }

        public void Update()
        {
            File.WriteAllText(_source, JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = { new StringEnumConverter() }
            }));
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

    internal class MainFormConfig
    {
        public Size Size { get; set; }
        public Point Location { get; set; }
        public FormWindowState WindowState { get; set; }
    }
}