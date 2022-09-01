using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVVM.Collections
{
    [Serializable]
    public class SyncReactiveList<T>
    {
        [SerializeField] private Object _target;
        [SerializeField] private string _propertyName;

        private ReactiveList<T> _property;

        public ReactiveList<T> Property =>
            _property ??= (ReactiveList<T>)Binders.GetProperty(_target, _propertyName);

        public void Subscribe(IReactiveListEventHandler<T> listener)
        {
            Property?.Subscribe(listener);
        }

        public void UnSubscribe(IReactiveListEventHandler<T> listener)
        {
            Property?.UnSubscribe(listener);
        }
    }
}