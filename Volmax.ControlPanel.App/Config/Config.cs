using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Volmax.ControlPanel.App.Config
{
    internal class Config
    {
        private readonly string _source;
        public Config(string source)
        {
            _source = source;
            JsonConvert.PopulateObject(File.ReadAllText(source), this);
            foreach (var (_, value) in Solutions)
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
            if (Solutions.TryGetValue(name, out var existing))
                return existing;
            return Solutions[name] = new SolutionConfig(this);
        }
    }
}