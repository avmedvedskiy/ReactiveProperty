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

        public static int GetListCapacity(this int count)
        {
            int num = (count == 0) ? 4 : (count * 2);

            if ((uint)num > 2146435071u)
                num = 2146435071;

            if (num < count)
                num = count;

            return num;
        }
    }
}