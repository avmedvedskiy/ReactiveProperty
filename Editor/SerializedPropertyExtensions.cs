using System;
using System.Linq;
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
            var methodInfo = value.GetType().GetMethod(methodName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
            int i = 0;
            Type searchedType;
            var parentManagedReferenceType = property.GetParentManagedReferenceType();
            if(parentManagedReferenceType == null)
                searchedType = property.GetScriptTypeFromProperty();
            else
            {
                searchedType = parentManagedReferenceType;
                i = 1;//ignore first element, because first element getted in parent property if it managed reference
            }
            
            FieldInfo info = null;
            for (; i < fields.Length; i++)
            {
                var fName = fields[i];
                info = searchedType.GetFieldDeep(fName);
                searchedType = info.FieldType;
            }
            return info;

        }

        private static Type GetParentManagedReferenceType(this SerializedProperty property)
        {
            string[] fields = property.propertyPath.Split('.');
            var parentProperty = property.serializedObject.FindProperty(fields.First());
            return parentProperty?.GetManagedReferenceType();
        }

        public static Type GetManagedReferenceType(this SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference)
            {
                return null;
            }

            return GetType(property.managedReferenceFullTypename);
        }

        static Type GetType(string typeName)
        {
            int splitIndex = typeName.IndexOf(' ');
            var assembly = Assembly.Load(typeName.Substring(0, splitIndex));
            return assembly.GetType(typeName.Substring(splitIndex + 1));
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