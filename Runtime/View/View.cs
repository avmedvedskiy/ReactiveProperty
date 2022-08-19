using UnityEngine;

namespace MVVM
{
    public abstract class View<T> : MonoBehaviour
    {
        [SerializeField] public SyncReactiveProperty<T> _target;
        [SerializeField] private bool _autoUpdateOnEnable = true;

        public void OnEnable()
        {
            Bind();
            if (_autoUpdateOnEnable)
                UpdateView(_target.Property.Value);
        }

        public void OnDisable()
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