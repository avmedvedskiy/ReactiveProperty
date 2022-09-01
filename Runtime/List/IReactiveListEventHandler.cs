namespace MVVM.Collections
{
    public interface IReactiveListEventHandler<T>
    {
        void OnAdd(T item);
        void OnClear();
        void OnInsert(int index, T item);
        void OnRemoveAt(int index);
        void OnReplace(int index, T item);
        void OnValueChanged(int index, T item);
    }
}