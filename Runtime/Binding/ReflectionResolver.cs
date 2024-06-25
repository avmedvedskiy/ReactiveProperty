using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVVM
{
    public class ReflectionResolver : IResolver
    {
        public IReactiveProperty Map(Object target, string name)
        {
            Debug.LogWarning($"Warning, used ReflectionResolver, Target:{target} name={name}");
            //add cache
            var type = target.GetType();
            var prop = type.GetProperty(name);
            if (prop == null)
            {
                var filed = type.GetField(name);
                return filed?.GetValue(target) as IReactiveProperty;
            }

            return prop.GetValue(target) as IReactiveProperty;
        }
    }
}