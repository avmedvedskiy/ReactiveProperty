using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MVVM
{
    [Serializable]
    public class ReactiveEvent : IReactiveEvent
    {
        private event Action OnEvent;

        public void Subscribe(Action action)
        {
            OnEvent += action;
        }

        public void UnSubscribe(Action action)
        {
            OnEvent -= action;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Notify()
        {
            OnEvent?.Invoke();
        }
    }
    
    [Serializable]
    public class ReactiveEvent<T> : IReadOnlyReactiveProperty<T>
    {
        private event Action<T> OnValueChanged;
        [SerializeField] private T _value;

        public ReactiveEvent()
        {
            _value = default;
        }

        public ReactiveEvent(T value)
        {
            _value = value;
        }

        public T Value => _value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Notify(T value)
        {
            _value = value;
            Notify();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetWithoutNotify(T value)
        {
            _value = value;
        }

        public void Subscribe(Action<T> action)
        {
            OnValueChanged += action;
        }

        public void UnSubscribe(Action<T> action)
        {
            OnValueChanged -= action;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Notify()
        {
            OnValueChanged?.Invoke(_value);
        }
    }
}