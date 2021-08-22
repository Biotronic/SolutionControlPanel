using System;
using System.Windows.Forms;
using SolutionControlPanel.App.Win32;
using SolutionControlPanel.App.Win32.Enums;

namespace SolutionControlPanel.App.Utils
{
    public abstract class PersistentForm : Form
    {
        private bool _initialized;
        private bool _updating;

        private void PersistentChanged()
        {
            if (!_initialized) return;
            if (_updating) return;
            OnPersistentChanged();
        }

        protected abstract void OnPersistentChanged();

        protected abstract void LoadPersistent();

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!_initialized)
            {
                _initialized = true;
                _updating = true;
                try
                {
                    LoadPersistent();
                }
                finally
                {
                    _updating = false;
                }
            }
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            PersistentChanged();
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            PersistentChanged();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            PersistentChanged();
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            PersistentChanged();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (!_initialized) return;
            switch ((WindowMessage)m.Msg)
            {
                case WindowMessage.WindowPosChanged:
                    PersistentChanged();
                    break;
                case WindowMessage.SysCommand:
                    switch ((SysCommand)m.WParam)
                    {
                        case SysCommand.Restore:
                        case SysCommand.Maximize:
                            PersistentChanged();
                            break;
                        case SysCommand.Minimize:
                        default: break;
                    }

                    break;
                case WindowMessage.ShowWindow:
                    switch ((ShowWindow)m.WParam)
                    {
                        case ShowWindow.Normal:
                        case ShowWindow.Restore:
                        case ShowWindow.Show:
                        case ShowWindow.ShowNA:
                        case ShowWindow.ShowNoActivate:
                        case ShowWindow.Maximized:
                            PersistentChanged();
                            break;
                    }
                    break;
            }
        }
    }
}
