using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MVVM
{
    [Serializable]
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private event Action<T> OnValueChanged;
        [SerializeField] private T _value;

        public ReactiveProperty()
        {
            _value = default;
        }

        public ReactiveProperty(T value)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    OnValueChanged?.Invoke(_value);
                }
            }
        }

        public void SetWithoutNotify(T value) => _value = value;

        internal void Set(T value)
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }

        public void Subscribe(Action<T> action)
        {
            OnValueChanged += action;
        }

        public void UnSubscribe(Action<T> action)
        {
            OnValueChanged -= action;
        }

        public static implicit operator T(ReactiveProperty<T> source)
        {
            return source._value;
        }
    }
}