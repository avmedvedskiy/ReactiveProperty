using System.Collections.Generic;
using UnityEngine;

namespace MVVM.Collections
{
    /// <summary>
    /// Abstract class for list view
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ListView<T> : MonoBehaviour, IReactiveListEventHandler<T>
    {
        [SerializeField] private bool _updateViewOnEnable = true;
        [SerializeField] private SyncReactiveList<T> _target;
        
        protected SyncReactiveList<T> Target => _target;
        
        protected virtual void OnEnable()
        {
            Bind();
            if (_updateViewOnEnable)
                OnInitialize(Target.Property.Values);
        }


        protected virtual void OnDisable()
        {
            UnBind();
        }

        private void Bind()
        {
            Target.Subscribe(this);
        }

        private void UnBind()
        {
            Target.UnSubscribe(this);
        }
        
        public abstract void OnAdd(T item);
        public abstract void OnClear();
        public abstract void OnInsert(int index, T item);
        public abstract void OnRemoveAt(int index);
        public abstract void OnReplace(int index, T item);
        public abstract void OnValueChanged(int index, T item);
        public abstract void OnInitialize(IReadOnlyList<T> values);
    }
}