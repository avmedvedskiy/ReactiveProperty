using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVVM
{
    [Serializable]
    public class SyncReactiveProperty
    {
        [SerializeField] private Object _target;
        [SerializeField] private string _propertyName;
            
            
        private IReactiveProperty _property;
        public IReactiveProperty GetSyncProperty()
        {
            _property ??= Binders.GetProperty(_target, _propertyName);
            return _property;
        }
    }
    
    public abstract class View<T> : MonoBehaviour
    {
        [SerializeField] private SyncReactiveProperty _target;
        
        private IReactiveProperty<T> _property;
        private IReactiveProperty<T> Property => _property ??= (IReactiveProperty<T>)_target.GetSyncProperty();
        public void OnEnable()
        {
            Bind();
        }

        public void OnDisable()
        {
            UnBind();
        }

        private void Bind()
        {
            Property?.Subscribe(UpdateView);
        }

        private void UnBind()
        {
            Property?.UnSubscribe(UpdateView);
        }

        protected abstract void UpdateView(T value);
    }
}