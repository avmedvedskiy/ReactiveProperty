using System;

namespace MVVM
{
    public static class WithExtensions
    {
        public static T With<T>(this T item, Action<T> action)
        {
            action.Invoke(item);
            return item;
        }
        
        public static T With<T>(this T item, Action<T> action, Func<bool> when)
        {
            if(when())
                action.Invoke(item);
            return item;
        }
    }
}