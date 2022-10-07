using System;

namespace MVVM
{
    public interface IReactiveProperty
    {
    }
    
    public interface IReactivePropertyValue : IReactiveProperty
    {
    }
    
    public interface IReadOnlyReactiveProperty<out T> : IReactivePropertyValue
    {
        T Value { get; }
        void Subscribe(Action<T> action);
        void UnSubscribe(Action<T> action);
    }

    public interface IReactiveProperty<T> : IReadOnlyReactiveProperty<T>
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
    
    public interface IReactiveList : IReactiveProperty
    {
    }
}