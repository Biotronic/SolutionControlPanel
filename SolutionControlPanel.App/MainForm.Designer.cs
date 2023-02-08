
namespace SolutionControlPanel.App
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.outputContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itmClearOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.itmRestoreOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.itmFile = new System.Windows.Forms.ToolStripMenuItem();
            this.itmChooseFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.itmShowAll = new System.Windows.Forms.ToolStripMenuItem();
            this.itmStartAtBoot = new System.Windows.Forms.ToolStripMenuItem();
            this.itmStartProjects = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.itmExit = new System.Windows.Forms.ToolStripMenuItem();
            this.autoStartTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.outputContextMenu.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(800, 404);
            this.splitContainer1.SplitterDistance = 238;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 238);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(796, 0);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.ContextMenuStrip = this.outputContextMenu;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(800, 162);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // outputContextMenu
            // 
            this.outputContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmClearOutput,
            this.itmRestoreOutput});
            this.outputContextMenu.Name = "contextMenuStrip1";
            this.outputContextMenu.Size = new System.Drawing.Size(153, 48);
            // 
            // itmClearOutput
            // 
            this.itmClearOutput.Name = "itmClearOutput";
            this.itmClearOutput.Size = new System.Drawing.Size(152, 22);
            this.itmClearOutput.Text = "&Clear output";
            this.itmClearOutput.Click += new System.EventHandler(this.ItmClearOutput_Click);
            // 
            // itmRestoreOutput
            // 
            this.itmRestoreOutput.Name = "itmRestoreOutput";
            this.itmRestoreOutput.Size = new System.Drawing.Size(152, 22);
            this.itmRestoreOutput.Text = "&Restore output";
            this.itmRestoreOutput.Click += new System.EventHandler(this.ItmRestoreOutput_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipTitle = null;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 428);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmFile});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(800, 24);
            this.mainMenuStrip.TabIndex = 2;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // itmFile
            // 
            this.itmFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmChooseFolder,
            this.itmShowAll,
            this.itmStartAtBoot,
            this.itmStartProjects,
            this.toolStripSeparator1,
            this.itmExit});
            this.itmFile.Name = "itmFile";
            this.itmFile.Size = new System.Drawing.Size(37, 20);
            this.itmFile.Text = "&File";
            this.itmFile.DropDownOpening += new System.EventHandler(this.itmFile_DropDownOpening);
            // 
            // itmChooseFolder
            // 
            this.itmChooseFolder.Name = "itmChooseFolder";
            this.itmChooseFolder.Size = new System.Drawing.Size(196, 22);
            this.itmChooseFolder.Text = "&Choose Root Folder...";
            this.itmChooseFolder.Click += new System.EventHandler(this.itmChooseFolder_Click);
            // 
            // itmShowAll
            // 
            this.itmShowAll.Name = "itmShowAll";
            this.itmShowAll.Size = new System.Drawing.Size(196, 22);
            this.itmShowAll.Text = "Show &all solutions";
            this.itmShowAll.Click += new System.EventHandler(this.itmShowAll_Click);
            // 
            // itmStartAtBoot
            // 
            this.itmStartAtBoot.CheckOnClick = true;
            this.itmStartAtBoot.Name = "itmStartAtBoot";
            this.itmStartAtBoot.Size = new System.Drawing.Size(196, 22);
            this.itmStartAtBoot.Text = "&Start with Windows";
            this.itmStartAtBoot.Click += new System.EventHandler(this.itmStartAtBoot_Click);
            // 
            // itmStartProjects
            // 
            this.itmStartProjects.CheckOnClick = true;
            this.itmStartProjects.Name = "itmStartProjects";
            this.itmStartProjects.Size = new System.Drawing.Size(196, 22);
            this.itmStartProjects.Text = "Start &projects at startup";
            this.itmStartProjects.Click += new System.EventHandler(this.itmStartProjects_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // itmExit
            // 
            this.itmExit.Name = "itmExit";
            this.itmExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.itmExit.Size = new System.Drawing.Size(196, 22);
            this.itmExit.Text = "E&xit";
            this.itmExit.Click += new System.EventHandler(this.itmExit_Click);
            // 
            // autoStartTimer
            // 
            this.autoStartTimer.Interval = 1000;
            this.autoStartTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "MainForm";
            this.Text = "Solution Control Panel";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.outputContextMenu.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ContextMenuStrip outputContextMenu;
        private System.Windows.Forms.ToolStripMenuItem itmClearOutput;
        private System.Windows.Forms.ToolStripMenuItem itmRestoreOutput;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem itmFile;
        private System.Windows.Forms.ToolStripMenuItem itmChooseFolder;
        private System.Windows.Forms.ToolStripMenuItem itmStartAtBoot;
        private System.Windows.Forms.ToolStripMenuItem itmStartProjects;
        private System.Windows.Forms.ToolStripMenuItem itmExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem itmShowAll;
        private System.Windows.Forms.Timer autoStartTimer;
    }
}

