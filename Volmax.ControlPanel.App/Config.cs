using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Volmax.ControlPanel.App
{
    internal class Config
    {
        private readonly string _source;
        public Config(string source)
        {
            _source = source;
            JsonConvert.PopulateObject(File.ReadAllText(source), this);
            foreach (var (key, value) in Solutions)
            {
                value.Config = this;
            }
        }

        public string Basepath { get; set; }
        public Dictionary<string, SolutionConfig> Solutions { get; set; }

        public void Update()
        {
            File.WriteAllText(_source, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public SolutionConfig GetSolution(string name)
        {
            if (Solutions.TryGetValue(name, out var _existing))
                return _existing;
            return Solutions[name] = new SolutionConfig(this);
        }
    }

    internal class SolutionConfig
    {
        internal Config Config;

        public SolutionConfig(Config config)
        {
            Config = config;
        }

        private string _profile;
        public string Profile
        {
            get => _profile;
            set
            {
                _profile = value;
                Config?.Update();
            }
        }

        private bool _checked;
        public bool Checked
        {
            get => _checked && !_hidden;
            set
            {
                _checked = value;
                Config?.Update();
            }
        }

        private bool _hidden;
        public bool Hidden
        {
            get => _hidden;
            set
            {
                _hidden = value;
                Config?.Update();
            }
        }
    }
}