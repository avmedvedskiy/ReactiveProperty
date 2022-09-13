using System;
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
                if (!Equals(_value, value))
                {
                    _value = value;
                    Notify();
                }
            }
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Notify()
        {
            OnValueChanged?.Invoke(_value);
        }
    }
}