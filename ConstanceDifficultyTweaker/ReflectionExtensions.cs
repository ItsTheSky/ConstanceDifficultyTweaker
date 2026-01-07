using System;
using System.Reflection;

namespace ConstanceDifficultyTweaker
{
    public static class ReflectionExtensions
    {
        extension(object obj)
        {
            public void SetPrivateField<T>(string fieldName, T value)
            {
                Type currentType = obj.GetType();
                
                while (currentType != null)
                {
                    var field = currentType.GetField(fieldName, 
                        BindingFlags.NonPublic | 
                        BindingFlags.Instance | 
                        BindingFlags.DeclaredOnly);
        
                    if (field != null)
                    {
                        field.SetValue(obj, value);
                        return;
                    }
        
                    currentType = currentType.BaseType;
                }
            }

            public T GetPrivateField<T>(string fieldName)
            {
                Type currentType = obj.GetType();
    
                while (currentType != null)
                {
                    var field = currentType.GetField(fieldName, 
                        BindingFlags.NonPublic | 
                        BindingFlags.Instance | 
                        BindingFlags.DeclaredOnly);
        
                    if (field != null)
                    {
                        return (T)field.GetValue(obj);
                    }
        
                    currentType = currentType.BaseType;
                }
    
                throw new Exception($"Field '{fieldName}' not found in type '{obj.GetType().FullName}' or its base types");
            }

            public void AddToArrayField<T>(string fieldName, T value)
            {
                var field = obj.GetType().GetField(fieldName,
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    var array = (T[])field.GetValue(obj);
                    var newArray = new T[array.Length + 1];
                    Array.Copy(array, newArray, array.Length);
                    newArray[array.Length] = value;
                    field.SetValue(obj, newArray);
                }
                else
                {
                    throw new Exception($"Field '{fieldName}' not found in type '{obj.GetType().FullName}'");
                }
            }

            public T CallPrivateMethod<T>(string methodName, params object[] parameters)
            {
                var method = obj.GetType().GetMethod(methodName,
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (method != null)
                {
                    return (T)method.Invoke(obj, parameters);
                }
                else
                {
                    throw new Exception($"Method '{methodName}' not found in type '{obj.GetType().FullName}'");
                }
            }

            public void CallPrivateMethod(string methodName, params object[] parameters)
            {
                var method = obj.GetType().GetMethod(methodName,
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (method != null)
                {
                    method.Invoke(obj, parameters);
                }
                else
                {
                    throw new Exception($"Method '{methodName}' not found in type '{obj.GetType().FullName}'");
                }
            }
        }
    }
}