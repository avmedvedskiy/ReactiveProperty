using UnityEngine;

namespace MVVM
{
    public abstract class View<T> : MonoBehaviour 
    {
        [SerializeField] private bool _updateViewOnEnable = true;
        [SerializeField] private SyncReactiveProperty<T> _target;

        protected SyncReactiveProperty<T> Target => _target;
        protected T CurrentValue => Target.Property.Value;
        protected bool UpdateViewOnEnable => _updateViewOnEnable;

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
            Target.Subscribe(UpdateView);
        }

        private void UnBind()
        {
            Target.UnSubscribe(UpdateView);
        }

        protected abstract void UpdateView(T value);
    }
}