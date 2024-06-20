using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVVM.Collections
{
    [Serializable]
    public class ReactiveList<T> : IReactiveList, IList<T>
    {
        [SerializeField] private List<T> _list;

        private IReactiveListEventHandler<T> _handler;

        public int Count => _list.Count;
        public bool IsReadOnly => false;

        public IReadOnlyList<T> Values => _list;

        public ReactiveList(int capacity) => _list = new List<T>(capacity);
        public ReactiveList() => _list = new List<T>();

        public void Add(T item)
        {
            _list.Add(item);
            _handler?.OnAdd(item);
        }

        public void Clear()
        {
            _list.Clear();
            _handler?.OnClear();
        }

        public bool Contains(T item) => _list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index < 0)
                return false;
            RemoveAt(index);
            return true;
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            _handler?.OnInsert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
            _handler?.OnRemoveAt(index);
        }

        public int FindIndex(Predicate<T> match)
        {
            int count = _list.Count;
            for (int i = 0; i < count; i++)
            {
                if (match(_list[i]))
                    return i;
            }
            return -1;
        }

        public T Find(Predicate<T> match)
        {
            int count = _list.Count;
            for (int i = 0; i < count; i++)
            {
                if (match(_list[i]))
                    return _list[i];
            }
            return default;
        }

        public void AddRange(IReadOnlyList<T> collection)
        {
            _list.AddRange(collection);
            _handler?.OnAddRange(collection);
        }

        public T this[int index]
        {
            get => _list[index];
            set
            {
                _list[index] = value;
                _handler?.OnValueChanged(index, value);
            }
        }

        public void ValueChanged(int index)
        {
            _handler?.OnValueChanged(index, _list[index]);
        }

        public void Replace(int index, T item)
        {
            _list[index] = item;
            _handler?.OnReplace(index, this[index]);
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Subscribe(IReactiveListEventHandler<T> handler)
            => _handler = handler;

        internal void UnSubscribe(IReactiveListEventHandler<T> handler)
            => _handler = null;
    }
}