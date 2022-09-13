namespace MVVM
{
    public static class ReactivePropertyExtensions
    {
        public static void FromProperty<T>(this ReactiveProperty<T> source, ReactiveProperty<T> reactiveProperty)
        {
            source.Value = reactiveProperty.Value;
            reactiveProperty.Subscribe(x => source.Value = x);
        }
    }
}