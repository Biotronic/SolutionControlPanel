using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SolutionControlPanel.App
{
    public partial class DialogBox : Form
    {
        private DialogBox()
        {
            InitializeComponent();
        }

        public static T Show<T>(string message, string caption, Dictionary<T, string> buttons, T defaultButton, string checkboxText, ref bool check)
        {
            var form = new DialogBox
            {
                Text = caption,
                label1 = { Text = message },
                checkBox1 =
                {
                    Checked = check,
                    Visible = !string.IsNullOrWhiteSpace(checkboxText),
                    Text = checkboxText
                }
            };

            var result = defaultButton;
            form.buttonGrid.ColumnCount = buttons.Count;
            var i = 0;
            foreach (var (key, buttonText) in buttons)
            {
                var btn = new Button
                {
                    Text = buttonText
                };
                btn.Click += (s, e) =>
                {
                    result = key;
                    form.Close();
                };
                form.buttonGrid.Controls.Add(btn, i, 0);
                ++i;
                if (Equals(key, defaultButton))
                {
                    form.AcceptButton = btn;
                }
            }

            var ownerHandle = GetActiveWindow();
            form.ShowDialog(FromHandle(ownerHandle));
            check = form.checkBox1.Checked;
            return result;
        }

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetActiveWindow();
    }
}
