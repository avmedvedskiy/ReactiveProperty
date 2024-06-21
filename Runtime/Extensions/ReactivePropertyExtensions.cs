namespace MVVM
{
    public static class ReactivePropertyExtensions
    {
        public static void Subscribe<T>(this ReactiveProperty<T> source, IReadOnlyReactiveProperty<T> reactiveProperty)
        {
            source.Value = reactiveProperty.Value;
            reactiveProperty.Subscribe(source.Set);
        }

        public static void UnSubscribe<T>(this ReactiveProperty<T> source, IReadOnlyReactiveProperty<T> reactiveProperty)
        {
            source.Value = reactiveProperty.Value;
            reactiveProperty.UnSubscribe(source.Set);
        }
    }
}