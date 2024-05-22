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
    }
}