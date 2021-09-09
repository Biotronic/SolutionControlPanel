using System;
using System.Windows.Forms;
using SolutionControlPanel.App.Properties;

namespace SolutionControlPanel.App
{
    public partial class GroupControl : UserControl
    {
        private int _selectedCount;
        public int SelectedCount
        {
            get => _selectedCount;
            set
            {
                _selectedCount = value;
                btnRestart.Text = @$"Start ({_selectedCount})";
                btnStop.Text = $@"Stop ({_selectedCount})";
                btnRestart.Enabled = _selectedCount > 0;
                btnStop.Enabled = _selectedCount > 0;
            }
        }

        public GroupControl()
        {
            InitializeComponent();
        }

        public event EventHandler Start;
        public event EventHandler Stop;

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Start?.Invoke(this, EventArgs.Empty);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop?.Invoke(this, EventArgs.Empty);
        }

        public void SetSelected(int count, bool allRunning)
        {
            _selectedCount = count;
            btnRestart.Text = allRunning ? $@"Restart ({_selectedCount})" : @$"Start ({_selectedCount})";
            btnStop.Text = $@"Stop ({_selectedCount})";
            btnRestart.Enabled = _selectedCount > 0;
            btnStop.Enabled = _selectedCount > 0;
        }
    }
}
