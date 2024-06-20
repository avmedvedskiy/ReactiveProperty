using System.Collections.Generic;

namespace MVVM.Collections
{
    public interface IReactiveListEventHandler<T>
    {
        void OnAddRange(IReadOnlyList<T> items);
        void OnAdd(T item);
        void OnClear();
        void OnInsert(int index, T item);
        void OnRemoveAt(int index);
        void OnReplace(int index, T item);
        void OnValueChanged(int index, T item);
    }
}