using UnityEngine;

namespace MVVM
{
    public abstract class EventView : MonoBehaviour
    {
        [SerializeField] private bool _updateViewOnEnable = true;
        [SerializeField] private SyncReactiveEvent _target;

        protected SyncReactiveEvent Target => _target;
        
        protected virtual void OnEnable()
        {
            Bind();
            if (_updateViewOnEnable)
                UpdateView();
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

        protected abstract void UpdateView();
    }
}