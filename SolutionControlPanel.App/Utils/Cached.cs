using System;

namespace SolutionControlPanel.App.Utils
{
    internal class Cached<T>
    {
        private readonly Func<T> _getter;
        private T _value;
        private DateTime _lastUpdate;
        private readonly TimeSpan _timeout;

        public Cached(Func<T> getter, TimeSpan? timeout = null)
        {
            _getter = getter;
            _lastUpdate = DateTime.MinValue;
            _timeout = timeout ?? TimeSpan.FromSeconds(10);
        }

        public T Value
        {
            get
            {
                if (_lastUpdate - DateTime.Now < _timeout)
                {
                    return _value;
                }
                _value = _getter();
                _lastUpdate = DateTime.Now;

                return _value;
            }
        }

        public static implicit operator T(Cached<T> cached)
        {
            return cached.Value;
        }
    }
}
