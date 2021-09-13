using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        private static T ShowCore<T>(
            string message, string caption, Dictionary<T, string> buttons, T defaultButton,
            Image image,
            string checkboxText, ref bool check)
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
                },
                pictureBox1 =
                {
                    BackgroundImage = image,
                    Size = image?.Size ?? Size.Empty
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

            form.ShowDialog(FromHandle(GetActiveWindow()));
            check = form.checkBox1.Checked;
            return result;
        }

        public static T Show<T>(string message, string caption, Dictionary<T, string> buttons, T defaultButton, string checkboxText, ref bool check, Image icon)
        {
            return ShowCore(message, caption, buttons, defaultButton, icon, checkboxText, ref check);
        }

        public static T Show<T>(string message, string caption, Dictionary<T, string> buttons, string checkboxText, ref bool check, Image icon)
        {
            var defaultButton = buttons.Keys.First();
            return ShowCore(message, caption, buttons, defaultButton, icon, checkboxText, ref check);
        }

        public static T Show<T>(string message, string caption, Dictionary<T, string> buttons, T defaultButton, string checkboxText, ref bool check)
        {
            return ShowCore(message, caption, buttons, defaultButton, null, checkboxText, ref check);
        }

        public static T Show<T>(string message, string caption, Dictionary<T, string> buttons, string checkboxText, ref bool check)
        {
            var defaultButton = buttons.Keys.First();
            return ShowCore(message, caption, buttons, defaultButton, null, checkboxText, ref check);
        }

        public static T Show<T>(string message, string caption, Dictionary<T, string> buttons, T defaultButton)
        {
            var _ = false;
            return ShowCore(message, caption, buttons, defaultButton, null, "", ref _);
        }

        public static T Show<T>(string message, string caption, Dictionary<T, string> buttons)
        {
            var _ = false;
            var defaultButton = buttons.Keys.First();
            return ShowCore(message, caption, buttons, defaultButton, null, "", ref _);
        }

        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons)
        {
            var _ = false;
            var (buttonDic, defaultButton) = ParseMessageBoxButtons(buttons);
            return ShowCore(message, caption, buttonDic, defaultButton, null, "", ref _);
        }

        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons, string checkboxText, ref bool check, Image icon)
        {
            var (buttonDic, defaultButton) = ParseMessageBoxButtons(buttons);
            return ShowCore(message, caption, buttonDic, defaultButton, icon, checkboxText, ref check);
        }

        private static (Dictionary<DialogResult, string> buttonDic, DialogResult defaultButton) ParseMessageBoxButtons(MessageBoxButtons buttons)
        {
            var buttonDic = new Dictionary<DialogResult, string>();
            DialogResult defaultButton;

            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    buttonDic[DialogResult.OK] = GetStandardString(DialogResult.OK);
                    defaultButton = DialogResult.OK;
                    break;
                case MessageBoxButtons.OKCancel:
                    buttonDic[DialogResult.OK] = GetStandardString(DialogResult.OK);
                    buttonDic[DialogResult.Cancel] = GetStandardString(DialogResult.Cancel);
                    defaultButton = DialogResult.OK;
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    buttonDic[DialogResult.Abort] = GetStandardString(DialogResult.Abort);
                    buttonDic[DialogResult.Retry] = GetStandardString(DialogResult.Retry);
                    buttonDic[DialogResult.Ignore] = GetStandardString(DialogResult.Ignore);
                    defaultButton = DialogResult.Abort;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    buttonDic[DialogResult.Yes] = GetStandardString(DialogResult.Yes);
                    buttonDic[DialogResult.No] = GetStandardString(DialogResult.No);
                    buttonDic[DialogResult.Cancel] = GetStandardString(DialogResult.Cancel);
                    defaultButton = DialogResult.Yes;
                    break;
                case MessageBoxButtons.YesNo:
                    buttonDic[DialogResult.Yes] = GetStandardString(DialogResult.Yes);
                    buttonDic[DialogResult.No] = GetStandardString(DialogResult.No);
                    defaultButton = DialogResult.Yes;
                    break;
                case MessageBoxButtons.RetryCancel:
                    buttonDic[DialogResult.Retry] = GetStandardString(DialogResult.Retry);
                    buttonDic[DialogResult.Cancel] = GetStandardString(DialogResult.Cancel);
                    defaultButton = DialogResult.Retry;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null);
            }

            return (buttonDic, defaultButton);
        }

        private static string GetStandardString(DialogResult value)
        {
            var id = (DialogBoxCommandIds)((int)value - 1);
            return Marshal.PtrToStringAuto(MB_GetString(id));
        }

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr MB_GetString(DialogBoxCommandIds strId);

        private enum DialogBoxCommandIds
        {
            IdNone = 0,
            IdOk = 1,
            IdCancel = 2,
            IdAbort = 3,
            IdRetry = 4,
            IdIgnore = 5,
            IdYes = 6,
            IdNo = 7,
            IdClose = 8,
            IdHelp = 9,
            IdTryAgain = 10,
            IdContinue = 11,
            IdTimeout = 32000
        }
    }
}
