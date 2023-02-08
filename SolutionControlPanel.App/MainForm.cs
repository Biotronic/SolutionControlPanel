using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SolutionControlPanel.App.Config;
using SolutionControlPanel.App.Processes;
using SolutionControlPanel.App.Properties;
using SolutionControlPanel.App.Utils;

namespace SolutionControlPanel.App
{
    public partial class MainForm : PersistentForm
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

            Config.MainForm ??= new MainFormConfig
            {
                Location = Location,
                Size = Size,
                WindowState = WindowState
            };

            itmStartAtBoot.Checked = RunAtStartup.Registered;
            itmStartProjects.Checked = Config.StartProjectsAutomatically;
            UpdateLists();
            autoStartTimer.Enabled = Config.StartProjectsAutomatically;
        }

        private void UpdateLists()
        {
            SuspendLayout();
            try
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
                    c.CheckAllClicked += Solution_CheckAll;
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
            finally
            {
                ResumeLayout();
            }
        }

        private void Solution_CheckAll(object sender, EventArgs e)
        {
            var newValue = !Solutions.All(a => a.Checked);
            foreach (var control in SolutionControls)
            {
                control.Checked = newValue;
            }
        }

        protected override void LoadPersistent()
        {
            Location = Config.MainForm.Location;
            Size = Config.MainForm.Size;
            WindowState = Config.MainForm.WindowState;
        }

        protected override void OnPersistentChanged()
        {
            Config.MainForm.WindowState = WindowState;
            Config.MainForm.Location = Location;
            Config.MainForm.Size = Size;
            Config.Update();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (_exitEarly)
            {
                Close();
                return;
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

        private void StopAllSolutions()
        {
            foreach (var control in SelectedSolutionControls)
            {
                control.Solution.Stop();
            }
        }

        private void StartAllSolutions()
        {
            foreach (var control in SelectedSolutionControls)
            {
                if (control.Solution.Status == SolutionStatus.Stopped)
                {
                    control.Solution.Start(control.Profile);
                }
            }
        }

        private void ShowAllSolutions()
        {
            SuspendLayout();
            try
            {
                foreach (var control in SolutionControls)
                {
                    control.Visible = true;
                }
            }
            finally
            {
                ResumeLayout();
            }
        }

        private void _groupControl_Start(object sender, EventArgs e)
        {
            StartAllSolutions();
        }

        private void _groupControl_Stop(object sender, EventArgs e)
        {
            StopAllSolutions();
        }

        private void Solution_CheckedChanged(object sender, EventArgs e)
        {
            _groupControl?.SetSelected(SelectedSolutionControls.Count(), SelectedSolutionControls.All(a => a.Solution.Status != SolutionStatus.Stopped));
        }

        private void ItmClearOutput_Click(object sender, EventArgs e)
        {
            CurrentSolutionControl?.ClearOutput();
        }

        private void ItmRestoreOutput_Click(object sender, EventArgs e)
        {
            CurrentSolutionControl?.RestoreOutput();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Solutions.All(a => a.Status == SolutionStatus.Stopped)) return;

            var neverAgain = Config.StopRunningOnExit.HasValue;
            var result = Config.StopRunningOnExit ?? DialogBox.Show(
                @"Do you want to stop running processes?",
                @"Closing",
                MessageBoxButtons.YesNoCancel,
                "Don't show again",
                ref neverAgain,
                SystemIcons.Question.ToBitmap());

            switch (result)
            {
                case DialogResult.Yes:
                    StopAllSolutions();
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (neverAgain)
            {
                Config.StopRunningOnExit = result;
            }
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
            RunAtStartup.Registered = itmStartAtBoot.Checked;
        }

        private void itmStartProjects_Click(object sender, EventArgs e)
        {
            Config.StartProjectsAutomatically = itmStartProjects.Checked;
            Config.Update();
        }

        private void itmShowAll_Click(object sender, EventArgs e)
        {
            ShowAllSolutions();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            autoStartTimer.Enabled = false;
            if (!Config.StartProjectsAutomatically) return;

            StartAllSolutions();
        }

        private void itmFile_DropDownOpening(object sender, EventArgs e)
        {
            itmStartAtBoot.Checked = RunAtStartup.Registered;
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start("explorer", e.LinkText);
        }
    }
}
