using System;

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

    public interface IReactiveEvent : IReactiveProperty
    {
        public void Subscribe(Action action);

        public void UnSubscribe(Action action);
    }
}