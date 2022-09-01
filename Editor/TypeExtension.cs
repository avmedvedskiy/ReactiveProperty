using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MVVM.Editor
{
    public static class TypeExtension
    {
        public static List<FieldInfo> GetAllFields<T>(this Type type)
        {
            return type
                .GetFields()
                .Where(f => !Attribute.IsDefined(f, typeof(IgnoreGenerationAttribute)) &&
                            f.FieldType.GetInterfaces().Contains(typeof(T)))
                .ToList();
        }

        public static List<PropertyInfo> GetAllProperties<T>(this Type type)
        {
            return type
                .GetProperties()
                .Where(f => !Attribute.IsDefined(f, typeof(IgnoreGenerationAttribute)) &&
                            f.PropertyType.GetInterfaces().Contains(typeof(T)))
                .ToList();
        }

        public static List<string> GetAllReactive<T>(this Type type)
        {
            var props =
                type.GetAllProperties<T>()
                    .Select(p => p.Name);

            var fields =
                type.GetAllFields<T>()
                    .Select(p => p.Name);

            return props
                .Concat(fields)
                .ToList();
        }

        public static List<string> GetAllReactive<T>(this Type type, Type genericType) where T: IReactiveProperty
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
    }
}