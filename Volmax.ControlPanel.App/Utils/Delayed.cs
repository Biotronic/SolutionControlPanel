using System;
using System.Diagnostics;
using System.Threading;

namespace Biotronic.SolutionControlPanel.App.Utils
{
    public class Delayed : IDisposable
    {
        private readonly Thread _thread;
        private readonly Action _action;
        private readonly TimeSpan _delay;
        private bool _aborted;

        public static Delayed Call(TimeSpan delay, Action action)
        {
            return new Delayed(delay, action);
        }

        public Delayed(TimeSpan delay, Action action)
        {
            _delay = delay;
            _action = action;
            _thread = new Thread(Run);
            _thread.Start();
        }

        private void Run()
        {
            try
            {
                Thread.Sleep(_delay);
                if (_aborted) return;
                _action.Invoke();
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        public void Abort()
        {
            _aborted = true;
        }

        public void Dispose()
        {
            Abort();
            _thread.Join();
        }
    }
}
