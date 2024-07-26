using UnityEngine;

namespace Utilities
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T instance;
        public static T Instance => instance;

        public static bool HasInstance => Instance != null;
        public static T TryGetInstance() => HasInstance ? Instance : null;

        private void Awake() => InitializeSingleton();

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) return;

            instance = this as T;
        }   
    }
}