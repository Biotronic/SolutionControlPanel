
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblSelected = new System.Windows.Forms.Label();
            this.lblSolutionName = new System.Windows.Forms.Label();
            this.lblStatusText = new System.Windows.Forms.Label();
            this.lblStatusIcon = new System.Windows.Forms.Label();
            this.cmbProfiles = new System.Windows.Forms.ComboBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itmName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.itmStart = new System.Windows.Forms.ToolStripMenuItem();
            this.itmStop = new System.Windows.Forms.ToolStripMenuItem();
            this.itmDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.itmRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.itmOpenSolution = new System.Windows.Forms.ToolStripMenuItem();
            this.itmOpenInBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.itmHide = new System.Windows.Forms.ToolStripMenuItem();
            this.checkallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 286F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblSelected, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSolutionName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStatusText, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStatusIcon, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbProfiles, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnStop, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnRestart, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(701, 28);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Click += new System.EventHandler(this.ClickOutside);
            // 
            // lblSelected
            // 
            this.lblSelected.AutoSize = true;
            this.lblSelected.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSelected.Location = new System.Drawing.Point(23, 0);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(10, 28);
            this.lblSelected.TabIndex = 7;
            this.lblSelected.Text = " ";
            this.lblSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSelected.Click += new System.EventHandler(this.ClickOutside);
            // 
            // lblSolutionName
            // 
            this.lblSolutionName.AutoSize = true;
            this.lblSolutionName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSolutionName.Location = new System.Drawing.Point(43, 0);
            this.lblSolutionName.Name = "lblSolutionName";
            this.lblSolutionName.Size = new System.Drawing.Size(10, 28);
            this.lblSolutionName.TabIndex = 0;
            this.lblSolutionName.Text = " ";
            this.lblSolutionName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSolutionName.Click += new System.EventHandler(this.ClickOutside);
            this.lblSolutionName.MouseHover += new System.EventHandler(this.lblSolutionName_MouseHover);
            // 
            // lblStatusText
            // 
            this.lblStatusText.AutoSize = true;
            this.lblStatusText.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblStatusText.Location = new System.Drawing.Point(442, 0);
            this.lblStatusText.Name = "lblStatusText";
            this.lblStatusText.Size = new System.Drawing.Size(78, 28);
            this.lblStatusText.TabIndex = 1;
            this.lblStatusText.Text = "Unresponsive";
            this.lblStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatusText.Click += new System.EventHandler(this.ClickOutside);
            this.lblStatusText.MouseHover += new System.EventHandler(this.lblStatusText_MouseHover);
            // 
            // lblStatusIcon
            // 
            this.lblStatusIcon.AutoSize = true;
            this.lblStatusIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblStatusIcon.Image = global::SolutionControlPanel.App.Properties.Resources.Offline_16x;
            this.lblStatusIcon.Location = new System.Drawing.Point(426, 0);
            this.lblStatusIcon.Name = "lblStatusIcon";
            this.lblStatusIcon.Size = new System.Drawing.Size(10, 28);
            this.lblStatusIcon.TabIndex = 5;
            this.lblStatusIcon.Text = " ";
            this.lblStatusIcon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatusIcon.Click += new System.EventHandler(this.ClickOutside);
            // 
            // cmbProfiles
            // 
            this.cmbProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProfiles.FormattingEnabled = true;
            this.cmbProfiles.Location = new System.Drawing.Point(140, 3);
            this.cmbProfiles.Name = "cmbProfiles";
            this.cmbProfiles.Size = new System.Drawing.Size(280, 23);
            this.cmbProfiles.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(623, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 22);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(542, 3);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(75, 22);
            this.btnRestart.TabIndex = 1;
            this.btnRestart.Text = "Start";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox1.Location = new System.Drawing.Point(3, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(14, 22);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            this.checkBox1.Click += new System.EventHandler(this.ClickOutside);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmName,
            this.toolStripSeparator1,
            this.itmStart,
            this.itmStop,
            this.itmDebug,
            this.itmRestart,
            this.itmOpenSolution,
            this.itmOpenInBrowser,
            this.toolStripSeparator2,
            this.itmHide,
            this.checkallToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 236);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // itmName
            // 
            this.itmName.Enabled = false;
            this.itmName.Name = "itmName";
            this.itmName.Size = new System.Drawing.Size(180, 22);
            this.itmName.Text = "Name";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // itmStart
            // 
            this.itmStart.Image = global::SolutionControlPanel.App.Properties.Resources.Run_16x;
            this.itmStart.Name = "itmStart";
            this.itmStart.Size = new System.Drawing.Size(180, 22);
            this.itmStart.Text = "&Start";
            this.itmStart.Click += new System.EventHandler(this.itmStart_Click);
            // 
            // itmStop
            // 
            this.itmStop.Image = global::SolutionControlPanel.App.Properties.Resources.Stop_16x;
            this.itmStop.Name = "itmStop";
            this.itmStop.Size = new System.Drawing.Size(180, 22);
            this.itmStop.Text = "&Stop";
            this.itmStop.Click += new System.EventHandler(this.itmStop_Click);
            // 
            // itmDebug
            // 
            this.itmDebug.Image = global::SolutionControlPanel.App.Properties.Resources.Debug_16x;
            this.itmDebug.Name = "itmDebug";
            this.itmDebug.Size = new System.Drawing.Size(180, 22);
            this.itmDebug.Text = "&Debug";
            this.itmDebug.Click += new System.EventHandler(this.itmDebug_Click);
            // 
            // itmRestart
            // 
            this.itmRestart.Image = global::SolutionControlPanel.App.Properties.Resources.Restart_16x;
            this.itmRestart.Name = "itmRestart";
            this.itmRestart.Size = new System.Drawing.Size(180, 22);
            this.itmRestart.Text = "&Restart";
            this.itmRestart.Click += new System.EventHandler(this.itmRestart_Click);
            // 
            // itmOpenSolution
            // 
            this.itmOpenSolution.Name = "itmOpenSolution";
            this.itmOpenSolution.Size = new System.Drawing.Size(180, 22);
            this.itmOpenSolution.Text = "&Open solution";
            this.itmOpenSolution.Click += new System.EventHandler(this.itmOpenSolution_Click);
            // 
            // itmOpenInBrowser
            // 
            this.itmOpenInBrowser.Name = "itmOpenInBrowser";
            this.itmOpenInBrowser.Size = new System.Drawing.Size(180, 22);
            this.itmOpenInBrowser.Text = "Open in &browser";
            this.itmOpenInBrowser.Click += new System.EventHandler(this.itmOpenInBrowser_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // itmHide
            // 
            this.itmHide.Image = global::SolutionControlPanel.App.Properties.Resources.Hide_16x;
            this.itmHide.Name = "itmHide";
            this.itmHide.Size = new System.Drawing.Size(180, 22);
            this.itmHide.Text = "&Hide";
            this.itmHide.Click += new System.EventHandler(this.itmHide_Click);
            // 
            // checkallToolStripMenuItem
            // 
            this.checkallToolStripMenuItem.Name = "checkallToolStripMenuItem";
            this.checkallToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.checkallToolStripMenuItem.Text = "Check &all";
            this.checkallToolStripMenuItem.Click += new System.EventHandler(this.checkallToolStripMenuItem_Click);
            // 
            // SolutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SolutionControl";
            this.Size = new System.Drawing.Size(701, 28);
            this.VisibleChanged += new System.EventHandler(this.SolutionControl_VisibleChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}
