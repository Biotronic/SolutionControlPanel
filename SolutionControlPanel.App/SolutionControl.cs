﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SolutionControlPanel.App.Processes;
using SolutionControlPanel.App.Properties;
using SolutionControlPanel.App.Utils;
using Timer = System.Timers.Timer;

namespace SolutionControlPanel.App
{
    public partial class SolutionControl : UserControl
    {
        private Solution _solution;
        public RichTextBox Textbox { get; set; }

        public bool Checked
        {
            get => Solution.Checked && Visible;
            set
            {
                checkBox1.Checked = Solution.Checked = value;
                CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler CheckedChanged;
        public event EventHandler HiddenChanged;
        public event EventHandler CheckAllClicked;

        internal Solution Solution
        {
            get => _solution;
            set
            {
                _solution = value;
                itmName.Text = Solution.Name;
                Checked = Solution.Checked;
                Visible = !Solution.Hidden;
                Solution.Update += UpdateSolution;
                Solution.OutputAdded += Solution_OutputAdded;

                cmbProfiles.Items.Clear();
                if (Solution.Profiles.Any())
                {
                    var profiles = Solution.Profiles.Keys.ToList();
                    cmbProfiles.Items.AddRange(profiles.OfType<object>().ToArray());
                    cmbProfiles.SelectedIndex = Math.Max(0, profiles.IndexOf(Solution.Profile));
                }
                else
                {
                    cmbProfiles.Items.Add("");
                    cmbProfiles.SelectedIndex = 0;
                    cmbProfiles.Enabled = false;
                    cmbProfiles.Width = 0;
                }

                itmOpenSolution.Image = Solution.Image;

                UpdateSolution(Solution, EventArgs.Empty);
            }
        }

        public string Profile => cmbProfiles.Text;

        private bool _current;
        public bool Current
        {
            get => _current;
            set
            {
                if (_current == value) return;
                _current = value;
                if (value)
                {
                    lblSelected.Text = @">";
                    BackColor = SystemColors.ControlDark;
                    var siblings = TopLevelControl?.Controls.Find(Name, true).Where(a => a != this).OfType<SolutionControl>() ?? Enumerable.Empty<SolutionControl>();
                    foreach (var sibling in siblings)
                    {
                        sibling.Current = false;
                    }
                    itmOpenInBrowser.Image = SystemDefaults.DefaultBrowserIcon;
                    Solution_OutputAdded(this, new TextEventArgs("", ""));
                }
                else
                {
                    lblSelected.Text = @" ";
                    BackColor = SystemColors.Control;
                }
            }
        }


        private readonly Timer _outputTimer;
        private void Solution_OutputAdded(object sender, TextEventArgs e)
        {
            _outputTimer.Stop();
            _outputTimer.Start();
        }

        public SolutionControl()
        {
            Control_ControlAdded(this, new ControlEventArgs(this));
            InitializeComponent();
            this.ParentChanged += SolutionControl_ParentChanged;
            _outputTimer = new Timer(250) { AutoReset = false };
            _outputTimer.Elapsed += (o, args) =>
            {
                Invoke(new Action(() => { timer1_Tick(o, EventArgs.Empty); }));
            };
        }

        private bool _hasNewParent;

        private void SolutionControl_ParentChanged(object sender, EventArgs e)
        {
            _hasNewParent = true;
        }

        private void Control_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.ControlAdded += Control_ControlAdded;
            e.Control.GotFocus += Control_GotFocus;
            e.Control.ContextMenuStrip = contextMenuStrip1;
            foreach (Control child in e.Control.Controls)
            {
                Control_ControlAdded(sender, new ControlEventArgs(child));
            }
        }

        private void Control_GotFocus(object sender, EventArgs e)
        {
            Current = true;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Solution.Restart(cmbProfiles.Text);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Solution.Stop();
        }

        private void lblStatusText_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(lblStatusText, Solution.Hint);
        }

        private void lblSolutionName_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(lblSolutionName, Solution.ProjectPath);
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(lblGitStatus, Solution.GitStatus);
        }

        private void UpdateSolution(object sender, EventArgs args)
        {
            void Action()
            {
                var attr = EnumEx.GetAttributes(Solution.Status).OfType<MarkAttribute>().First();
                lblStatusIcon.Text = @"    ";
                lblStatusIcon.Image = attr.Image;
                lblStatusText.Text = $@"{attr.Text} ({Solution.Processes.Count()}/{Solution.ExpectedProcessCount})";

                lblSolutionName.Text = Solution.Name;

                btnStop.Enabled = Solution.Status != SolutionStatus.Stopped;
                btnRestart.Text = Solution.Status == SolutionStatus.Stopped ? @"Start" : @"Restart";
                btnRestart.Image = Solution.Status == SolutionStatus.Stopped
                    ? Resources.Run_16x
                    : Resources.Restart_16x;

                lblGitStatus.Text = Solution.GitShortStatus;
            }

            if (InvokeRequired)
            {
                Invoke((Action)Action);
            }
            else
            {
                Action();
            }
        }

        private void ClickOutside(object sender, EventArgs e)
        {
            btnRestart.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Solution.Checked = checkBox1.Checked;
            CheckedChanged?.Invoke(this, e);
        }

        private void itmHide_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void itmStart_Click(object sender, EventArgs e)
        {
            Solution.Start(cmbProfiles.Text);
        }

        private void itmRestart_Click(object sender, EventArgs e)
        {
            Solution.Restart(cmbProfiles.Text);
        }

        private void itmStop_Click(object sender, EventArgs e)
        {
            Solution.Stop();
        }

        private void itmDebug_Click(object sender, EventArgs e)
        {
            Solution.AttachDebugger();
        }

        private void SolutionControl_VisibleChanged(object sender, EventArgs e)
        {
            if (_hasNewParent)
            {
                _hasNewParent = false;
                return;
            }
            Solution.Hidden = !Visible;
            HiddenChanged?.Invoke(this, e);
            if (!Visible)
            {
                Checked = false;
            }
            else
            {
                CheckedChanged?.Invoke(this, e);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            itmStart.Visible = Solution.Status == SolutionStatus.Stopped;
            itmRestart.Visible = Solution.Status != SolutionStatus.Stopped;
            itmStop.Visible = Solution.Status != SolutionStatus.Stopped;
            itmDebug.Visible = Solution.Status != SolutionStatus.Debugged && Solution.Status != SolutionStatus.Stopped;
        }

        public void ClearOutput()
        {
            Solution.ClearOutput();
            Solution_OutputAdded(this, new TextEventArgs("", ""));
        }

        public void RestoreOutput()
        {
            Solution.RestoreOutput();
            Solution_OutputAdded(this, new TextEventArgs("", ""));
        }

        private void itmOpenSolution_Click(object sender, EventArgs e)
        {
            Solution.Open();
        }

        private void itmOpenInBrowser_Click(object sender, EventArgs e)
        {
            Solution.OpenInBrowser();
        }

        private void checkallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckAllClicked?.Invoke(this, EventArgs.Empty);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Textbox == null || !Current) { return; }

            Textbox.SuspendLayout();
            Textbox.Rtf = Solution.RichText;
            Textbox.SelectionStart = Textbox.Text.Length;
            Textbox.ScrollToCaret();
            Textbox.ResumeLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Solution.GitPull();
        }

        private void openInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Solution.OpenInExplorer();
        }
    }
}
