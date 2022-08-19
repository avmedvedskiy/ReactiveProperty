using System;
using UnityEngine;

namespace MVVM
{
    public interface IReactiveProperty
    {
    }

    public interface IReactiveProperty<T> : IReactiveProperty
    {
        T Value { get; set; }
        void Subscribe(Action<T> action);
        void UnSubscribe(Action<T> action);
    }

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
                    OnValueChanged?.Invoke(_value);
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
    }
}