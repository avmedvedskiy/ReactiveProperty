using System;
using System.Reflection;
using UnityEditor;

namespace MVVM.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static void InvokeMethod(this SerializedProperty property, string methodName)
        {
            var info = property.GetFieldInfo();
            Object obj = property.GetTargetObject();
            var value = info.GetValue(obj);
            var methodInfo = value.GetType().GetMethod(methodName,BindingFlags.Public| BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(value, Array.Empty<object>());
        }
        
        public static FieldInfo GetFieldDeep(this Type type, string fieldName)
        {
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null)
                field = type.BaseType.GetFieldDeep(fieldName);
            return field;
        }

        private static FieldInfo GetFieldInfo(this SerializedProperty property)
        {
            string[] fields = property.propertyPath.Split('.');
            Type typeFromProperty = GetScriptTypeFromProperty(property);
            FieldInfo info = null;
            foreach (var fName in fields)
            {
                info = typeFromProperty.GetFieldDeep(fName);
                typeFromProperty = info.FieldType;
            }
            return info;
        }
        
        public static Type GetTargetType(this SerializedProperty property)
        {
            return property.GetFieldInfo().FieldType;
        }
        
        public static Type GetGenericTypeArguments(this SerializedProperty property)
        {
            var type = property.GetTargetType();
            return type.GenericTypeArguments.Length > 0 ? type.GenericTypeArguments[0] : null;
        }
        
        public static Type GetScriptTypeFromProperty(this SerializedProperty property)
        {
            return property.serializedObject.targetObject.GetType();
        }
        
        public static Object GetTargetObject(this SerializedProperty property)
        {
            return property.serializedObject.targetObject;
        }
    }
}