using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVVM
{
    [Serializable]
    public class SyncReactiveEvent : ISyncReactive
    {
        [SerializeField] private Object _target;
        [SerializeField] private string _propertyName;

        private IReactiveEvent _property;
        public bool IsNull() => _target == null || string.IsNullOrEmpty(_propertyName);

        public IReactiveEvent Property =>
            _property ??= (IReactiveEvent)Binders.GetProperty(_target, _propertyName);

        public void Subscribe(Action action)
        {
            Property?.Subscribe(action);
        }

        public void UnSubscribe(Action action)
        {
            Property?.UnSubscribe(action);
        }
    }
}