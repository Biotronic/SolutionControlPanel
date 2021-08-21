﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Biotronic.SolutionControlPanel.App.Config;
using Biotronic.SolutionControlPanel.App.Processes;
using Biotronic.SolutionControlPanel.App.Properties;
using Biotronic.SolutionControlPanel.App.Utils;

namespace Biotronic.SolutionControlPanel.App
{
    public partial class MainForm : Form
    {
        private GroupControl _groupControl;
        private readonly bool _exitEarly;
        private Config.Config Config { get; }

        public MainForm()
        {
            InitializeComponent();

            Config = new Config.Config("config.json");
            if (Config.Basepath == null)
            {
                _exitEarly = true;
                return;
            }

            if (Config.MainForm == null)
            {
                Config.MainForm = new MainFormConfig
                {
                    Location = Location,
                    Size = Size,
                    WindowState = WindowState
                };
            }

            itmStartAtBoot.Checked = RunAtStartup.Registered;
            itmStartProjects.Checked = Config.StartProjectsAutomatically;
            UpdateLists();
            timer1.Enabled = true;
        }

        private void UpdateLists()
        {
            Text = @$"{Resources.AppName} - {Config.Basepath}";
            if (_groupControl != null)
            {
                tableLayoutPanel1.Controls.Remove(_groupControl);
                _groupControl.Start -= _groupControl_Start;
                _groupControl.Stop -= _groupControl_Stop;
            }

            Solutions = Solution.GetSolutions(Config).ToList();
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
                return;
            }

            if (_first)
            {
                var tmpLocation = Config.MainForm.Location;
                var tmpSize = Config.MainForm.Size;
                Location = tmpLocation;
                Size = tmpSize;
                WindowState = Config.MainForm.WindowState;
            }

            if (_first && Environment.GetCommandLineArgs().Contains(Resources.AtStartup))
            {
                Hide();
            }
            _first = false;
        }
        private bool _first = true;

        private IEnumerable<SolutionControl> SolutionControls => tableLayoutPanel1.Controls.OfType<SolutionControl>();
        private IEnumerable<SolutionControl> SelectedSolutionControls => SolutionControls.Where(a => a.Checked);
        private SolutionControl CurrentSolutionControl => SolutionControls.FirstOrDefault(a => a.Current);

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

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void itmChooseFolder_Click(object sender, EventArgs e)
        {
            if (Config.SetBasePath())
            {
                UpdateLists();
            }
        }

        private void itmExit_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void itmStartAtBoot_Click(object sender, EventArgs e)
        {
            if (itmStartAtBoot.Checked)
            {
                RunAtStartup.Register();
            }
            else
            {
                RunAtStartup.Unregister();
            }
        }

        private void itmStartProjects_Click(object sender, EventArgs e)
        {
            Config.StartProjectsAutomatically = itmStartProjects.Checked;
            Config.Update();
        }

        private void itmShowAll_Click(object sender, EventArgs e)
        {
            foreach (var control in SolutionControls)
            {
                control.Visible = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (!Config.StartProjectsAutomatically) return;

            _groupControl_Start(this, e);
        }

        private void itmFile_DropDownOpening(object sender, EventArgs e)
        {
            itmStartAtBoot.Checked = RunAtStartup.Registered;
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            if (_first) return;
            Config.MainForm.Size = Size;
            Config.Update();
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            if (_first) return;
            Config.MainForm.Location = Location;
            Config.Update();
        }

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_RESTORE = 0xF120;
        const int SC_MINIMIZE = 0xF020;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                switch ((int)m.WParam)
                {
                    case SC_MINIMIZE:
                        break;
                    case SC_RESTORE:
                        Config.MainForm.WindowState = FormWindowState.Normal;
                        break;
                    case SC_MAXIMIZE:
                        Config.MainForm.WindowState = FormWindowState.Maximized;
                        break;
                }
            }
            base.WndProc(ref m);
        }
    }
}
