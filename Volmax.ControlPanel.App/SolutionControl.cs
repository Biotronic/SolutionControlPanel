using System;
using System.Linq;
using System.Windows.Forms;
using Volmax.ControlPanel.App.Processes;
using Volmax.ControlPanel.App.Utils;

namespace Volmax.ControlPanel.App
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
                checkBox1.Checked = value;
                Solution.Checked = value;
            }
        }

        public event EventHandler CheckedChanged;
        public event EventHandler HiddenChanged;
        public event EventHandler ShowAll;

        internal Solution Solution
        {
            get => _solution;
            set
            {
                _solution = value;
                Checked = Solution.Checked;
                Visible = !Solution.Hidden;
                Solution.Update += UpdateSolution;
                Solution.OutputAdded += Solution_OutputAdded;

                cmbProfiles.Items.Clear();
                if (!Solution.Profiles.Any())
                {
                    cmbProfiles.Items.Add("");
                    cmbProfiles.SelectedIndex = 0;
                    cmbProfiles.Enabled = false;
                    cmbProfiles.Width = 0;
                }
                else
                {
                    var profiles = Solution.Profiles.Keys.ToList();
                    cmbProfiles.Items.AddRange(profiles.ToArray());
                    cmbProfiles.SelectedIndex = profiles.IndexOf(Solution.Profile);
                    if (cmbProfiles.SelectedIndex == -1)
                    {
                        cmbProfiles.SelectedIndex = 0;
                    }
                }

                UpdateSolution(Solution, EventArgs.Empty);
            }
        }

        public string Profile => cmbProfiles.Text;

        private void Solution_OutputAdded(object sender, TextEventArgs e)
        {
            Invoke((Action)(() =>
               {
                   if (Textbox != null && ContainsFocus)
                   {
                       Textbox.Rtf = Solution.RichText;
                   }
               }));
        }

        public SolutionControl()
        {
            Control_ControlAdded(this, new ControlEventArgs(this));
            InitializeComponent();
        }

        private void Control_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.ControlAdded += Control_ControlAdded;
            e.Control.GotFocus += Control_GotFocus;
            e.Control.LostFocus += Control_LostFocus;
            e.Control.ContextMenuStrip = contextMenuStrip1;
            foreach (Control child in e.Control.Controls)
            {
                Control_ControlAdded(sender, new ControlEventArgs(child));
            }
        }

        private void Control_LostFocus(object sender, EventArgs e)
        {
            lblSelected.Text = @" ";
        }

        private void Control_GotFocus(object sender, EventArgs e)
        {
            lblSelected.Text = @">";
            Solution_OutputAdded(this, new TextEventArgs("", ""));
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

        private void UpdateSolution(object sender, EventArgs args)
        {
            void Action()
            {
                var attr = EnumEx.GetAttributes(Solution.Status).OfType<MarkAttribute>().First();
                lblStatusIcon.Text = @"    ";
                lblStatusIcon.Image = attr.Image;
                lblStatusText.Text = $@"{attr.Text} ({Solution.Processes.Count()}/{Solution.ExpectedProcessCount})";

                lblSelected.Text = @" ";

                lblSolutionName.Text = Solution.Name;

                btnStop.Enabled = Solution.Status != SolutionStatus.Stopped;
                btnRestart.Text = Solution.Status == SolutionStatus.Stopped ? @"Start" : @"Restart";
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

        private void itmShowAll_Click(object sender, EventArgs e)
        {
            ShowAll?.Invoke(this, e);
        }

        private void SolutionControl_VisibleChanged(object sender, EventArgs e)
        {
            Solution.Hidden = !Visible;
            HiddenChanged?.Invoke(this, e);
            CheckedChanged?.Invoke(this, e);
        }
    }
}
