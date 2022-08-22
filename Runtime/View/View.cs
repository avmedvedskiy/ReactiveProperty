using UnityEngine;

namespace MVVM
{
    public abstract class View<T> : MonoBehaviour
    {
        [SerializeField] private bool _updateViewOnEnable = true;
        [SerializeField] private SyncReactiveProperty<T> _target;

        protected SyncReactiveProperty<T> Target => _target;
        protected T CurrentValue => _target.Property.Value;
        
        protected virtual void OnEnable()
        {
            Bind();
            if (_updateViewOnEnable)
                UpdateView(CurrentValue);
        }

        protected virtual void OnDisable()
        {
            UnBind();
        }

        private void Bind()
        {
            _target.Subscribe(UpdateView);
        }

        private void UnBind()
        {
            _target.UnSubscribe(UpdateView);
        }

        protected abstract void UpdateView(T value);
    }
}