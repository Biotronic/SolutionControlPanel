
namespace SolutionControlPanel.App
{
    partial class DialogBox
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainGrid = new System.Windows.Forms.TableLayoutPanel();
            this.buttonGrid = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.mainGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainGrid
            // 
            this.mainGrid.AutoSize = true;
            this.mainGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainGrid.ColumnCount = 3;
            this.mainGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
            this.mainGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
            this.mainGrid.Controls.Add(this.buttonGrid, 1, 1);
            this.mainGrid.Controls.Add(this.label1, 0, 0);
            this.mainGrid.Controls.Add(this.checkBox1, 0, 2);
            this.mainGrid.Location = new System.Drawing.Point(0, 0);
            this.mainGrid.Name = "mainGrid";
            this.mainGrid.RowCount = 3;
            this.mainGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainGrid.Size = new System.Drawing.Size(90, 116);
            this.mainGrid.TabIndex = 0;
            // 
            // buttonGrid
            // 
            this.buttonGrid.AutoSize = true;
            this.buttonGrid.ColumnCount = 1;
            this.buttonGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttonGrid.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.buttonGrid.Location = new System.Drawing.Point(34, 48);
            this.buttonGrid.Name = "buttonGrid";
            this.buttonGrid.Padding = new System.Windows.Forms.Padding(10);
            this.buttonGrid.RowCount = 1;
            this.buttonGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttonGrid.Size = new System.Drawing.Size(20, 40);
            this.buttonGrid.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.mainGrid.SetColumnSpan(this.label1, 3);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.label1.Size = new System.Drawing.Size(58, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.mainGrid.SetColumnSpan(this.checkBox1, 3);
            this.checkBox1.Location = new System.Drawing.Point(4, 94);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(83, 19);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // DialogBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(734, 221);
            this.Controls.Add(this.mainGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DialogBox";
            this.Text = "DialogBox";
            this.mainGrid.ResumeLayout(false);
            this.mainGrid.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainGrid;
        private System.Windows.Forms.TableLayoutPanel buttonGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}