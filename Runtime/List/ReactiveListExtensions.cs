using System;
using System.Collections.Generic;

namespace MVVM.Collections
{
    public static class ReactiveListExtensions
    {
        public static bool TryGetIndex<T>(this IList<T> list, Predicate<T> match, out int index)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                if (match(list[i]))
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }
        
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

        public static void FromReactiveList<T>(this ReactiveList<T> source, ReactiveList<T> list)
        {
            foreach (var item in list)
            {
                source.Add(item);
            }
            
            list.OnAdd += source.Add;
            list.OnClear += source.Clear;
            list.OnInsert += source.Insert;
            list.OnRemoveAt += source.RemoveAt;
            list.OnReplace += source.Replace;
            list.OnValueChanged += (i, item) =>
            {
                source[i] = item;
            };
        }
    }
}