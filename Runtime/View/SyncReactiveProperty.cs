using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVVM
{
    [Serializable]
    public class SyncReactiveProperty<T>
    {
        [SerializeField] private Object _target;
        [SerializeField] private string _propertyName;
        
        private IReactiveProperty<T> _property;

        public IReactiveProperty<T> Property =>
            _property ??= (IReactiveProperty<T>)Binders.GetProperty(_target, _propertyName);

        public void Subscribe(Action<T> action)
        {
            Property?.Subscribe(action);
        }

        public void UnSubscribe(Action<T> action)
        {
            Property?.UnSubscribe(action);
        }
    }
}