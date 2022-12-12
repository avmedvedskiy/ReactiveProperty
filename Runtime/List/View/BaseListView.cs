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
    public abstract class BaseListView<TModel, TView> : ListView<TModel>
        where TView : ModelView<TModel>
    {
        [SerializeField] private TView _template;
        [SerializeField] private Transform _root;
        [SerializeField] private List<TView> _views = new();

        protected List<TView> Views => _views;
        protected Transform Root => _root;

        public override void OnAdd(TModel item) =>
            _views.Add(InstantiateView(item));

        public override void OnClear()
        {
            _views.ForEach(DestroyView);
            _views.Clear();
        }

        public override void OnInsert(int index, TModel item) =>
            _views.Insert(index,
                InstantiateView(item)
                    .With(x => x.transform.SetSiblingIndex(index)));

        public override void OnRemoveAt(int index)
        {
            DestroyView(_views[index]);
            _views.RemoveAt(index);
        }

        public override void OnReplace(int index, TModel item) =>
            _views[index].SetModel(item);
        //for struct or classes
        /*
        DestroyView(_views[index]);
        _views[index] = Instantiate(template, _root)
            .With(x => x.transform.SetSiblingIndex(index))
            .With(x => x.SetValue(item));
            */
        //for simple type 

        public override void OnValueChanged(int index, TModel item) => _views[index].SetModel(item);

        public override void OnInitialize(IReadOnlyList<TModel> values)
        {
            //need to init only one time
            if(_views.Count != 0)
                return;
            
            foreach (var value in values)
            {
                _views.Add(InstantiateView(value));
            }
        }

        protected virtual TView InstantiateView(TModel item) =>
            Instantiate(_template, _root)
                .With(x => x.SetModel(item));

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