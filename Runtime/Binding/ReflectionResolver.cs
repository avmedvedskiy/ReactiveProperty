using UnityEngine;

namespace MVVM
{
    public class ReflectionResolver : IResolver
    {
        public IReactiveProperty Map(Object target, string name)
        {
            //add cache
            var prop = target.GetType().GetProperty(name);
            if (prop == null)
            {
                var filed = target.GetType().GetField(name);
                return filed?.GetValue(target) as IReactiveProperty;
            }
            else
            {
                return prop.GetValue(target) as IReactiveProperty;
            }
        }
    }
}