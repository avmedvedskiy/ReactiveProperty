#if UNITY_6000_0_OR_NEWER
using System.Collections.Generic;
using UnityEngine;

namespace MVVM.Collections
{
    public abstract class BaseListViewAsync<TModel, TView> : MonoBehaviour, IReactiveListEventHandler<TModel>
        where TView : ModelView<TModel>
    {
        [SerializeField] private SyncReactiveList<TModel> _target = new();
        [SerializeField] protected bool _updateViewOnEnable = true;
        [SerializeField] private TView _template;
        [SerializeField] private Transform _root;
        [SerializeField] private List<TView> _views = new();

        protected SyncReactiveList<TModel> Target => _target;
        protected List<TView> Views => _views;
        protected Transform Root => _root;

        protected virtual void OnEnable()
        {
            _target.Subscribe(this);

            if (_updateViewOnEnable)
                OnInitialize(_target.Property.Values);
        }

        protected virtual void OnDisable()
        {
            _target.UnSubscribe(this);
        }

        public void OnAdd(TModel item)
        {
            var instantiateOperation = InstantiateAsync(_template, _root);
            instantiateOperation.completed += x =>
            {
                var result = instantiateOperation.Result[0];
                _views.Add(result);
                OnViewInstantiated(result, item);
            };
        }

        public void OnAddRange(IReadOnlyList<TModel> items)
        {
            var itemsCount = items.Count;
            _views.Capacity = (_views.Count + itemsCount).GetListCapacity();
            var instantiateOperation = InstantiateAsync(_template, itemsCount, _root);
            instantiateOperation.completed += x =>
            {
                for (int j = 0; j < itemsCount; j++)
                {
                    var result = instantiateOperation.Result[j];
                    _views.Add(result);
                    OnViewInstantiated(result, items[j]);
                }
            };
        }

        public void OnClear()
        {
            _views.ForEach(DestroyView);
            _views.Clear();
        }

        public void OnInsert(int index, TModel item)
        {
            var instantiateOperation = InstantiateAsync(_template, _root);
            instantiateOperation.completed += x =>
            {
                var result = instantiateOperation.Result[0];
                _views.Insert(index, result);
                OnViewInstantiated(result, item);
                result.transform.SetSiblingIndex(index);
            };
        }

        private void OnViewInstantiated(TView view, TModel model)
        {
            view.transform.localPosition = Vector3.zero;
            view.transform.localScale = Vector3.one;
            view.transform.localRotation = Quaternion.identity;
            view.SetModel(model);
        }

        public void OnRemoveAt(int index)
        {
            DestroyView(_views[index]);
            _views.RemoveAt(index);
        }

        public void OnReplace(int index, TModel item)
            => _views[index].SetModel(item);

        public void OnValueChanged(int index, TModel item)
            => _views[index].SetModel(item);

        public void OnInitialize(IReadOnlyList<TModel> items)
        {
            //need to init only one time
            if (_views.Count != 0)
                return;

            if (items.Count > 0)
                OnAddRange(items);
        }

        protected virtual void DestroyView(TView view)
        {
            Destroy(view.gameObject);
        }

        protected virtual void OnValidate()
        {
            if (_root == null)
                _root = transform;
        }
    }
}
#endif