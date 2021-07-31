using System;
using System.Windows.Forms;

namespace Volmax.ControlPanel.App
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
                btnRestart.Text = @$"Start ({SelectedCount})";
                btnStop.Text = $@"Stop ({SelectedCount})";
                btnRestart.Enabled = SelectedCount > 0;
                btnStop.Enabled = SelectedCount > 0;
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
    }
}
