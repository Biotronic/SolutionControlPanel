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

        public MainForm()
        {
            InitializeComponent();

            var config = new Config.Config("config.json");

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

        private void Solution_ShowAll(object sender, EventArgs e)
        {
            foreach (var control in tableLayoutPanel1.Controls.OfType<SolutionControl>())
            {
                control.Visible = true;
            }
        }

        private void _groupControl_Start(object sender, EventArgs e)
        {
            foreach (var control in tableLayoutPanel1.Controls.OfType<SolutionControl>().Where(a => a.Checked))
            {
                control.Solution.Restart(control.Profile);
            }
        }

        private void _groupControl_Stop(object sender, EventArgs e)
        {
            foreach (var control in tableLayoutPanel1.Controls.OfType<SolutionControl>().Where(a => a.Checked))
            {
                control.Solution.Stop();
            }
        }

        private void Solution_CheckedChanged(object sender, EventArgs e)
        {
            if (_groupControl != null)
            {
                _groupControl.SelectedCount = tableLayoutPanel1.Controls.OfType<SolutionControl>().Count(a => a.Checked);
            }
        }

        internal List<Solution> Solutions { get; set; }
    }
}
