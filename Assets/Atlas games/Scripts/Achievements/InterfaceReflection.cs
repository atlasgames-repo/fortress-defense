using System;
using System.Collections.Generic;
using System.Reflection;

public static class InterfaceReflection
{
    public static IEnumerable<T> GetClassesImplementingInterface<T>()
    {
        Type interfaceType = typeof(T);

        // Get all loaded assemblies
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
        {
            // Get all types in the assembly
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                // Check if the type implements the desired interface
                if (interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    // Create an instance of the type
                    T instance = (T)Activator.CreateInstance(type);
                    yield return instance;
                }
            }
        }
    }
}