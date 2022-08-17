using UnityEngine;

namespace MVVM
{
    public class ReflectionResolver : IResolver
    {
        public IReactiveProperty Map(Object target, string name)
        {
            //add cache
            return target.GetType().GetProperty(name)?.GetValue(target) as IReactiveProperty;
        }
    }
}