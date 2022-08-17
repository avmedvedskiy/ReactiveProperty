using UnityEngine;
using Object = UnityEngine.Object;

namespace MVVM
{
    public abstract class View<T> : MonoBehaviour
    {
        public Object Target;
        public string PropName;

        private IReactiveProperty<T> _property;
        
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
            _property ??= Binders.GetProperty(Target, PropName) as ReactiveProperty<T>;
            _property?.Subscribe(UpdateView);
        }

        private void UnBind()
        {
            _property?.UnSubscribe(UpdateView);
        }

        protected abstract void UpdateView(T value);
    }
}