using System;
using UnityEngine;

namespace MVVM
{
    public interface IReactiveProperty
    {

    }
    public interface IReactiveProperty<T> : IReactiveProperty
    {
        public void Subscribe(Action<T> action);
        public void UnSubscribe(Action<T> action);
    }

    [Serializable]
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private event Action<T> _onValueChanged;
        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    _onValueChanged?.Invoke(_value);
                }
            }
        }

        public void Subscribe(Action<T> action)
        {
            _onValueChanged += action;
            action.Invoke(Value);
        }

        public void UnSubscribe(Action<T> action)
        {
            _onValueChanged -= action;
        }
    }
}