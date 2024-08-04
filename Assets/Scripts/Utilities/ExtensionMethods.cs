using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Utilities
{
    public static class ExtensionMethods
    {
        public static T OrNull<T>(this T obj) where T : Object => obj != null ? obj : null;

        public static void IsNotNull<T>(this T obj, Action action)
        {
            if (obj != null)
                action();
        }

        public static bool IsNotNull<T>(this T obj, Func<bool> func)
        {
            return obj != null && func();
        }

        public static void IsNull<T>(this T obj, Action action)
        {
            if (obj == null)
                action();
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }
    }
}