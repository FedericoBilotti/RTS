using UnityEngine;

namespace Utilities
{
    public static class ExtensionsGameObject
    {
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>().OrNull() ?? gameObject.AddComponent<T>().GetComponent<T>();
        }

        /// <summary>
        /// Checks if the given gameObject has a component of type T in its actual GameObject or its children
        /// </summary>
        /// <param name="gameObject">GameObject to check</param>
        /// <param name="component">Component to check</param>
        /// <typeparam name="T">The component type</typeparam>
        /// <returns></returns>
        public static bool TryGetComponentInGOAndChildren<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponent<T>();
            
            if (component) return true;

            component = gameObject.GetComponentInChildren<T>();

            return component;
        }

        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponentInChildren<T>();
            return component;
        }

        public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponentInParent<T>();
            return component;
        }
    }
}