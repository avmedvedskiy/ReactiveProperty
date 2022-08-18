using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVM.Editor
{
    public static class TypeExtension
    {
        public static List<string> GetAllReactive(this Type type)
        {
            var props = 
                type
                    .GetProperties()
                    .Where(f => f.PropertyType.GetInterfaces().Contains(typeof(IReactiveProperty)))
                    .Select(p=> p.Name)
                    .ToList();
            
            var fields = 
                type
                    .GetFields()
                    .Where(f => f.FieldType.GetInterfaces().Contains(typeof(IReactiveProperty)))
                    .Select(p=> p.Name)
                    .ToList();
            props.AddRange(fields);
            return props;
        }
    }
}