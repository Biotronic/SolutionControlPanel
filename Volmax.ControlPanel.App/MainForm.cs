using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Volmax.ControlPanel.App.Processes;

namespace Volmax.ControlPanel.App
{
    public partial class MainForm : Form
    {
        private readonly GroupControl _groupControl;
        private readonly bool _exitEarly;

        public MainForm()
        {
            InitializeComponent();

            var config = new Config.Config("config.json");
            if (config.Basepath == null)
            {
                _exitEarly = true;
                return;
            }

            Solutions = Solution.GetSolutions(config).ToList();
            tableLayoutPanel1.RowCount = Solutions.Count + 1;
            foreach (var solution in Solutions)
            {
                var c = new SolutionControl
                {
                    Solution = solution,
                    Dock = DockStyle.Top,
                    Textbox = richTextBox1
                };
                c.CheckedChanged += Solution_CheckedChanged;
                c.ShowAll += Solution_ShowAll;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableLayoutPanel1.Controls.Add(c);
            }
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _groupControl = new GroupControl
            {
                Dock = DockStyle.Top
            };
            tableLayoutPanel1.Controls.Add(_groupControl);
            _groupControl.Start += _groupControl_Start;
            _groupControl.Stop += _groupControl_Stop;
            Solution_CheckedChanged(this, EventArgs.Empty);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (_exitEarly)
            {
                Close();
            }
        }

        private IEnumerable<SolutionControl> SolutionControls => tableLayoutPanel1.Controls.OfType<SolutionControl>();
        private IEnumerable<SolutionControl> SelectedSolutionControls => SolutionControls.Where(a => a.Checked);
        private SolutionControl CurrentSolutionControl => SolutionControls.FirstOrDefault(a => a.Current);

        private void Solution_ShowAll(object sender, EventArgs e)
        {
            foreach (var control in SolutionControls)
            {
                control.Visible = true;
            }
        }

        private void _groupControl_Start(object sender, EventArgs e)
        {
            foreach (var control in SelectedSolutionControls)
            {
                if (control.Solution.Status == SolutionStatus.Stopped)
                {
                    control.Solution.Start(control.Profile);
                }
            }
        }

        private void _groupControl_Stop(object sender, EventArgs e)
        {
            foreach (var control in SelectedSolutionControls)
            {
                control.Solution.Stop();
            }
        }

        private void Solution_CheckedChanged(object sender, EventArgs e)
        {
            if (_groupControl != null)
            {
                _groupControl.SelectedCount = SelectedSolutionControls.Count();
            }
        }

        private void ItmClearOutput_Click(object sender, EventArgs e)
        {
            CurrentSolutionControl?.ClearOutput();
        }

        private void ItmRestoreOutput_Click(object sender, EventArgs e)
        {
            CurrentSolutionControl?.RestoreOutput();
        }

        internal List<Solution> Solutions { get; set; }
    }
}
