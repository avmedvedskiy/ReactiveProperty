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
}