using System;
using System.Threading;

namespace Volmax.ControlPanel.App
{
    public class Delayed
    {
        private readonly Thread _thread;
        private readonly Action _action;
        private readonly TimeSpan _delay;
        private bool _aborted = false;

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
            { }
        }

        public void Abort()
        {
            _aborted = true;
        }
    }
}
