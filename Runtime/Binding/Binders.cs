using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVVM
{
    public static class Binders
    {
        private static Dictionary<Type, IResolver> _resolvers = new();

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
                return target == null ? null : _reflectionResolver.Map(target, name);
            }

            return _resolvers[type].Map(target, name);
        }
    }
}