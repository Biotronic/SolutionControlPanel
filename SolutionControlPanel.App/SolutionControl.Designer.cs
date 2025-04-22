
using System.Windows.Forms;

namespace SolutionControlPanel.App
{
    partial class SolutionControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblGitStatus = new Label();
            lblSelected = new Label();
            lblSolutionName = new Label();
            lblStatusText = new Label();
            lblStatusIcon = new Label();
            cmbProfiles = new ComboBox();
            btnStop = new Button();
            btnRestart = new Button();
            checkBox1 = new CheckBox();
            button1 = new Button();
            toolTip1 = new ToolTip(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            itmName = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            itmStart = new ToolStripMenuItem();
            itmStop = new ToolStripMenuItem();
            itmDebug = new ToolStripMenuItem();
            itmRestart = new ToolStripMenuItem();
            itmOpenSolution = new ToolStripMenuItem();
            itmOpenInBrowser = new ToolStripMenuItem();
            openInExplorerToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            itmHide = new ToolStripMenuItem();
            checkallToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 10;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 286F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(lblGitStatus, 8, 0);
            tableLayoutPanel1.Controls.Add(lblSelected, 1, 0);
            tableLayoutPanel1.Controls.Add(lblSolutionName, 1, 0);
            tableLayoutPanel1.Controls.Add(lblStatusText, 4, 0);
            tableLayoutPanel1.Controls.Add(lblStatusIcon, 3, 0);
            tableLayoutPanel1.Controls.Add(cmbProfiles, 2, 0);
            tableLayoutPanel1.Controls.Add(btnStop, 7, 0);
            tableLayoutPanel1.Controls.Add(btnRestart, 6, 0);
            tableLayoutPanel1.Controls.Add(checkBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(button1, 9, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1225, 28);
            tableLayoutPanel1.TabIndex = 0;
            tableLayoutPanel1.Click += ClickOutside;
            // 
            // lblGitStatus
            // 
            lblGitStatus.AutoSize = true;
            lblGitStatus.Dock = DockStyle.Left;
            lblGitStatus.Location = new System.Drawing.Point(1101, 0);
            lblGitStatus.Name = "lblGitStatus";
            lblGitStatus.Size = new System.Drawing.Size(62, 28);
            lblGitStatus.TabIndex = 9;
            lblGitStatus.Text = "Up to date";
            lblGitStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblGitStatus.MouseHover += label1_MouseHover;
            // 
            // lblSelected
            // 
            lblSelected.AutoSize = true;
            lblSelected.Dock = DockStyle.Left;
            lblSelected.Location = new System.Drawing.Point(39, 0);
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new System.Drawing.Size(10, 28);
            lblSelected.TabIndex = 7;
            lblSelected.Text = " ";
            lblSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblSelected.Click += ClickOutside;
            // 
            // lblSolutionName
            // 
            lblSolutionName.AutoSize = true;
            lblSolutionName.Dock = DockStyle.Left;
            lblSolutionName.Location = new System.Drawing.Point(23, 0);
            lblSolutionName.Name = "lblSolutionName";
            lblSolutionName.Size = new System.Drawing.Size(10, 28);
            lblSolutionName.TabIndex = 0;
            lblSolutionName.Text = " ";
            lblSolutionName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblSolutionName.Click += ClickOutside;
            lblSolutionName.MouseHover += lblSolutionName_MouseHover;
            // 
            // lblStatusText
            // 
            lblStatusText.AutoSize = true;
            lblStatusText.Dock = DockStyle.Left;
            lblStatusText.Location = new System.Drawing.Point(807, 0);
            lblStatusText.Name = "lblStatusText";
            lblStatusText.Size = new System.Drawing.Size(78, 28);
            lblStatusText.TabIndex = 1;
            lblStatusText.Text = "Unresponsive";
            lblStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblStatusText.Click += ClickOutside;
            lblStatusText.MouseHover += lblStatusText_MouseHover;
            // 
            // lblStatusIcon
            // 
            lblStatusIcon.AutoSize = true;
            lblStatusIcon.Dock = DockStyle.Left;
            lblStatusIcon.Image = Properties.Resources.Offline_16x;
            lblStatusIcon.Location = new System.Drawing.Point(791, 0);
            lblStatusIcon.Name = "lblStatusIcon";
            lblStatusIcon.Size = new System.Drawing.Size(10, 28);
            lblStatusIcon.TabIndex = 5;
            lblStatusIcon.Text = " ";
            lblStatusIcon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblStatusIcon.Click += ClickOutside;
            // 
            // cmbProfiles
            // 
            cmbProfiles.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProfiles.FormattingEnabled = true;
            cmbProfiles.Location = new System.Drawing.Point(505, 3);
            cmbProfiles.Name = "cmbProfiles";
            cmbProfiles.Size = new System.Drawing.Size(280, 23);
            cmbProfiles.TabIndex = 0;
            // 
            // btnStop
            // 
            btnStop.Image = Properties.Resources.Stop_16x;
            btnStop.Location = new System.Drawing.Point(1004, 3);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(91, 22);
            btnStop.TabIndex = 2;
            btnStop.Text = "Stop";
            btnStop.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnRestart
            // 
            btnRestart.Image = Properties.Resources.Run_16x;
            btnRestart.Location = new System.Drawing.Point(907, 3);
            btnRestart.Name = "btnRestart";
            btnRestart.Size = new System.Drawing.Size(91, 22);
            btnRestart.TabIndex = 1;
            btnRestart.Text = "Start";
            btnRestart.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRestart.UseVisualStyleBackColor = true;
            btnRestart.Click += btnRestart_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Dock = DockStyle.Left;
            checkBox1.Location = new System.Drawing.Point(3, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(14, 22);
            checkBox1.TabIndex = 8;
            checkBox1.Text = "checkBox1";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            checkBox1.Click += ClickOutside;
            // 
            // button1
            // 
            button1.Image = Properties.Resources.Pull_16x;
            button1.Location = new System.Drawing.Point(1201, 3);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(21, 22);
            button1.TabIndex = 10;
            toolTip1.SetToolTip(button1, "Pull");
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { itmName, toolStripSeparator1, itmStart, itmStop, itmDebug, itmRestart, itmOpenSolution, itmOpenInBrowser, openInExplorerToolStripMenuItem, toolStripSeparator2, itmHide, checkallToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(163, 236);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // itmName
            // 
            itmName.Enabled = false;
            itmName.Name = "itmName";
            itmName.Size = new System.Drawing.Size(162, 22);
            itmName.Text = "Name";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // itmStart
            // 
            itmStart.Image = Properties.Resources.Run_16x;
            itmStart.Name = "itmStart";
            itmStart.Size = new System.Drawing.Size(162, 22);
            itmStart.Text = "&Start";
            itmStart.Click += itmStart_Click;
            // 
            // itmStop
            // 
            itmStop.Image = Properties.Resources.Stop_16x;
            itmStop.Name = "itmStop";
            itmStop.Size = new System.Drawing.Size(162, 22);
            itmStop.Text = "&Stop";
            itmStop.Click += itmStop_Click;
            // 
            // itmDebug
            // 
            itmDebug.Image = Properties.Resources.Debug_16x;
            itmDebug.Name = "itmDebug";
            itmDebug.Size = new System.Drawing.Size(162, 22);
            itmDebug.Text = "&Debug";
            itmDebug.Click += itmDebug_Click;
            // 
            // itmRestart
            // 
            itmRestart.Image = Properties.Resources.Restart_16x;
            itmRestart.Name = "itmRestart";
            itmRestart.Size = new System.Drawing.Size(162, 22);
            itmRestart.Text = "&Restart";
            itmRestart.Click += itmRestart_Click;
            // 
            // itmOpenSolution
            // 
            itmOpenSolution.Name = "itmOpenSolution";
            itmOpenSolution.Size = new System.Drawing.Size(162, 22);
            itmOpenSolution.Text = "&Open solution";
            itmOpenSolution.Click += itmOpenSolution_Click;
            // 
            // itmOpenInBrowser
            // 
            itmOpenInBrowser.Name = "itmOpenInBrowser";
            itmOpenInBrowser.Size = new System.Drawing.Size(162, 22);
            itmOpenInBrowser.Text = "Open in &browser";
            itmOpenInBrowser.Click += itmOpenInBrowser_Click;
            // 
            // openInExplorerToolStripMenuItem
            // 
            openInExplorerToolStripMenuItem.Name = "openInExplorerToolStripMenuItem";
            openInExplorerToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            openInExplorerToolStripMenuItem.Text = "Open in E&xplorer";
            openInExplorerToolStripMenuItem.Click += openInExplorerToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(159, 6);
            // 
            // itmHide
            // 
            itmHide.Image = Properties.Resources.Hide_16x;
            itmHide.Name = "itmHide";
            itmHide.Size = new System.Drawing.Size(162, 22);
            itmHide.Text = "&Hide";
            itmHide.Click += itmHide_Click;
            // 
            // checkallToolStripMenuItem
            // 
            checkallToolStripMenuItem.Name = "checkallToolStripMenuItem";
            checkallToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            checkallToolStripMenuItem.Text = "Check &all";
            checkallToolStripMenuItem.Click += checkallToolStripMenuItem_Click;
            // 
            // SolutionControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "SolutionControl";
            Size = new System.Drawing.Size(1225, 28);
            VisibleChanged += SolutionControl_VisibleChanged;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblSolutionName;
        private System.Windows.Forms.Label lblStatusText;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Label lblStatusIcon;
        private System.Windows.Forms.ComboBox cmbProfiles;
        private System.Windows.Forms.Label lblSelected;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private ToolTip toolTip1;
        private CheckBox checkBox1;
        private ToolStripMenuItem itmHide;
        private ToolStripMenuItem itmStart;
        private ToolStripMenuItem itmStop;
        private ToolStripMenuItem itmDebug;
        private ToolStripMenuItem itmRestart;
        private ToolStripMenuItem itmName;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem itmOpenSolution;
        private ToolStripMenuItem itmOpenInBrowser;
        private ToolStripMenuItem checkallToolStripMenuItem;
        private Label lblGitStatus;
        private Button button1;
        private ToolStripMenuItem openInExplorerToolStripMenuItem;
    }
}
