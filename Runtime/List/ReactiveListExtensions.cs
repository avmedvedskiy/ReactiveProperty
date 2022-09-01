namespace MVVM.Collections
{
    public static class ReactiveListExtensions
    {
        public static void Subscribe<T>(this ReactiveList<T> list, IReactiveListEventHandler<T> listener)
        {
            list.OnAdd += listener.OnAdd;
            list.OnClear += listener.OnClear;
            list.OnInsert += listener.OnInsert;
            list.OnRemoveAt += listener.OnRemoveAt;
            list.OnReplace += listener.OnReplace;
            list.OnValueChanged += listener.OnValueChanged;
        }

        public static void UnSubscribe<T>(this ReactiveList<T> list, IReactiveListEventHandler<T> listener)
        {
            list.OnAdd -= listener.OnAdd;
            list.OnClear -= listener.OnClear;
            list.OnInsert -= listener.OnInsert;
            list.OnRemoveAt -= listener.OnRemoveAt;
            list.OnReplace -= listener.OnReplace;
            list.OnValueChanged -= listener.OnValueChanged;
        }
    }
}