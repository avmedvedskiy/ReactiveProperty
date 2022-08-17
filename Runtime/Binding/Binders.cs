using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVVM
{
    public static class Binders
    {
        private static Dictionary<Type, IResolver> _resolvers;

        public static void AddResolvers(Dictionary<Type, IResolver> additionalResolvers)
        {
            _resolvers = additionalResolvers;
        }

        private static readonly ReflectionResolver _reflectionResolver = new();
        public static IReactiveProperty GetProperty(Object target, string name)
        {
            var type = target.GetType();
            if (!_resolvers.ContainsKey(type))
            {
                Debug.LogError("Warning, used ReflectionResolver, please check codogen");
                return _reflectionResolver.Map(target, name);
            }
            return _resolvers[type].Map(target, name);
        }
    }
}