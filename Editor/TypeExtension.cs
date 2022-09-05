using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MVVM.Editor
{
    public static class TypeExtension
    {
        public static List<FieldInfo> GetAllFields<TInterface>(this Type type)
        {
            return type
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => !Attribute.IsDefined(f, typeof(IgnoreGenerationAttribute)) &&
                            f.FieldType.GetInterfaces().Contains(typeof(TInterface)))
                .ToList();
        }

        
        public static List<PropertyInfo> GetAllProperties<TInterface>(this Type type)
        {
            return type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => !Attribute.IsDefined(f, typeof(IgnoreGenerationAttribute)) &&
                            f.PropertyType.GetInterfaces().Contains(typeof(TInterface)))
                .ToList();
        }

        public static List<string> GetAllReactive<TInterface>(this Type type)
        {
            var props =
                type.GetAllProperties<TInterface>()
                    .Select(p => p.Name);

            var fields =
                type.GetAllFields<TInterface>()
                    .Select(p => p.Name);

            return props
                .Concat(fields)
                .ToList();
        }

        public static List<string> GetAllReactive<T>(this Type type, Type genericType) where T : IReactiveProperty
        {
            bool FilterPropertyInfoByGeneric(Type f)
            {
                if (genericType != null)
                    return f.GenericTypeArguments.Length > 0 &&
                           f.GenericTypeArguments[0] == genericType;

                return f.GenericTypeArguments.Length == 0; //если пришло нулл, то искать все у кого генериков 0
            }

            var props =
                type.GetAllProperties<T>()
                    .Where((f) => FilterPropertyInfoByGeneric(f.PropertyType))
                    .Select(p => p.Name);

            var fields =
                type.GetAllFields<T>()
                    .Where((f) => FilterPropertyInfoByGeneric(f.FieldType))
                    .Select(p => p.Name);

            return props
                .Concat(fields)
                .ToList();
        }


        public static Type GenericTypeArgumentDeep(this Type type)
        {
            if (type == null)
                return null;

            return type.GenericTypeArguments.Length > 0
                ? type.GenericTypeArguments[0]
                : GenericTypeArgumentDeep(type.BaseType);
        }
    }
}