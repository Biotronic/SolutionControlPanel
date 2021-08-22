namespace SolutionControlPanel.App.Config
{
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