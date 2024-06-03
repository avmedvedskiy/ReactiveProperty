namespace MVVM
{
    public static class ReactivePropertyExtensions
    {
        /// <summary>
        /// Not useful now, memory leaked
        /// </summary>
        public static void FromProperty<T>(this ReactiveProperty<T> source, IReadOnlyReactiveProperty<T> reactiveProperty)
        {
            source.Value = reactiveProperty.Value;
            reactiveProperty.Subscribe(x => source.Value = x);
        }
        
        public static void Subscribe<T>(this ReactiveProperty<T> source, IReadOnlyReactiveProperty<T> reactiveProperty)
        {
            reactiveProperty.Subscribe(source.Set);
        }
        
        public static void UnSubscribe<T>(this ReactiveProperty<T> source, IReadOnlyReactiveProperty<T> reactiveProperty)
        {
            reactiveProperty.UnSubscribe(source.Set);
        }
    }
}