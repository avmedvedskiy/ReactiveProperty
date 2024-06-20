using System.Collections.Generic;
using UnityEngine;

namespace MVVM.Collections
{
    /// <summary>
    /// Base realization for ReactiveList, contain View Component(ModelView) and Model type
    /// you can make nested classes or use it for simple views
    /// In Nested classes override InstantiateView and DestroyView methods for addressables/animatons/pools etc
    /// </summary>
    /// <typeparam name="TView">View Component, should be nested from ModelView</typeparam>
    /// <typeparam name="TModel">Model</typeparam>
    public abstract class BaseListView<TModel, TView> : MonoBehaviour, IReactiveListEventHandler<TModel>
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

        public void OnAdd(TModel item) =>
            _views.Add(InstantiateView(item));

        public void OnAddRange(IReadOnlyList<TModel> items)
        {
            var itemsCount = items.Count;
            _views.Capacity = (_views.Count + itemsCount).GetListCapacity();

            for (int i = 0; i < itemsCount; i++)
                OnAdd(items[i]);
        }

        public void OnClear()
        {
            _views.ForEach(DestroyView);
            _views.Clear();
        }

        public void OnInsert(int index, TModel item)
        {
            var view = InstantiateView(item);
            _views.Insert(index, view);
            view.transform.SetSiblingIndex(index);
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

        protected virtual TView InstantiateView(TModel item)
        {
            var view = Instantiate(_template, _root);
            view.SetModel(item);
            return view;
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