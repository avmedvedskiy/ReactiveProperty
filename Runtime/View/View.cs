using UnityEngine;

namespace MVVM
{
    public interface IView
    {
        bool IsTargetSet();
        bool IsPropertyEquals(string propertyName);
    }

    public abstract class View<T> : MonoBehaviour, IView
    {
        [SerializeField] private bool _updateViewOnEnable = true;
        [SerializeField] private SyncReactiveProperty<T> _target = new();

        protected SyncReactiveProperty<T> Target => _target;
        protected T CurrentValue => Target.Property.Value;
        protected bool UpdateViewOnEnable => _updateViewOnEnable;

        protected virtual void OnEnable()
        {
            _target.Subscribe(UpdateView);

            if (_updateViewOnEnable)
                UpdateView(CurrentValue);
        }

        protected virtual void OnDisable()
        {
            _target.UnSubscribe(UpdateView);
        }

        protected abstract void UpdateView(T value);
        public bool IsTargetSet() => _target != null && !_target.IsNull();
        public bool IsPropertyEquals(string propertyName) => IsTargetSet() && _target.IsPropertyEquals(propertyName);
    }
}