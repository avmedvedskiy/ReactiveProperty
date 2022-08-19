using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MVVM.Editor
{
    public static class TypeExtension
    {
        public static FieldInfo GetFieldDeep(this Type type, string fieldName)
        {
            var field = type.GetField(fieldName);
            if (field == null)
                field =type.BaseType.GetFieldDeep(fieldName);
            return field;
        }
        
        public static List<string> GetAllReactive(this Type type)
        {
            var props =
                type
                    .GetProperties()
                    .Where(f => f.PropertyType.GetInterfaces().Contains(typeof(IReactiveProperty)))
                    .Select(p => p.Name);

            var fields =
                type
                    .GetFields()
                    .Where(f => f.FieldType.GetInterfaces().Contains(typeof(IReactiveProperty)))
                    .Select(p => p.Name);

            return props
                .Concat(fields)
                .ToList();
                
        }

        public static List<string> GetAllReactive(this Type type, Type genericType)
        {
            var props =
                type
                    .GetProperties()
                    .Where(f => f.PropertyType.GetInterfaces().Contains(typeof(IReactiveProperty)))
                    .Where(f => f.PropertyType.GenericTypeArguments.Length > 0 && f.PropertyType.GenericTypeArguments[0] == genericType)
                    .Select(p => p.Name);

            var fields =
                type
                    .GetFields()
                    .Where(f => f.FieldType.GetInterfaces().Contains(typeof(IReactiveProperty)))
                    .Where(f => f.FieldType.GenericTypeArguments.Length > 0 && f.FieldType.GenericTypeArguments[0] == genericType)
                    .Select(p => p.Name);

            return props
                .Concat(fields)
                .ToList();
        }
    }
}