using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVVM.Collections
{
    [Serializable]
    public class ReactiveList<T> : IReactiveList, IList<T>
    {
        [SerializeField] private List<T> _list = new();

        public event Action<T> OnAdd;
        public event Action OnClear;
        public event Action<int, T> OnInsert;
        public event Action<int> OnRemoveAt;
        public event Action<int, T> OnReplace;
        public event Action<int, T> OnValueChanged;


        public int Count => _list.Count;
        public bool IsReadOnly => false;

        public IReadOnlyList<T> Values => _list;

        public void Add(T item)
        {
            _list.Add(item);
            OnAdd?.Invoke(item);
        }

        public void Clear()
        {
            _list.Clear();
            OnClear?.Invoke();
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
            OnInsert?.Invoke(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
            OnRemoveAt?.Invoke(index);
        }

        public T this[int index]
        {
            get => _list[index];
            set
            {
                _list[index] = value;
                OnReplace?.Invoke(index, value);
            }
        }

        public void ValueChanged(int index)
        {
            OnValueChanged?.Invoke(index, this[index]);
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}