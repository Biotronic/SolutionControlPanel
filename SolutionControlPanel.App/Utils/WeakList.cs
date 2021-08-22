using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SolutionControlPanel.App.Utils
{
    public class WeakList<T> : ICollection<T> where T : class
    {
        private readonly List<WeakReference<T>> _references = new List<WeakReference<T>>();

        public IEnumerator<T> GetEnumerator()
        {
            return _references.SelectMany(a =>
            {
                if (a.TryGetTarget(out var x))
                {
                    return new[] { x };
                }
                return null;
            }).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _references.Add(new WeakReference<T>(item));
        }

        public void Clear()
        {
            _references.Clear();
        }

        public bool Contains(T item)
        {
            return this.Any(a => a == item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                if (arrayIndex >= array.Length) continue;

                array[arrayIndex] = item;
                ++arrayIndex;
            }
        }

        public bool Remove(T item)
        {
            var index = RawIndexOf(item);
            if (index <= -1) return false;
            _references.RemoveAt(index);
            return true;

        }

        public int Count => this.Count();
        public bool IsReadOnly => false;

        private int RawIndexOf(T item)
        {
            var i = 0;
            foreach (var reference in _references)
            {
                if (reference.TryGetTarget(out var x))
                {
                    if (x == item)
                    {
                        return i;
                    }
                }
                ++i;
            }
            return -1;
        }
    }
}
