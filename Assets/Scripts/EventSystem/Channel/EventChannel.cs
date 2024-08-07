using System.Collections.Generic;
using EventSystem.Listener;
using UnityEngine;

namespace EventSystem
{
    public abstract class EventChannel<T> : ScriptableObject
    {
        private readonly HashSet<EventListener<T>> _gameEventListeners = new();

        public void Invoke(T value)
        {
            foreach (EventListener<T> eventListener in _gameEventListeners)
            {
                eventListener.Raise(value);
            }
        }

        public void RegisterListener(EventListener<T> listener) => _gameEventListeners.Add(listener);
        public void UnregisterListener(EventListener<T> listener) => _gameEventListeners.Remove(listener);
    }

    [CreateAssetMenu(menuName = "EventSystem/EmptyEvent")]
    public class EventChannel : EventChannel<Empty> { }

    public readonly struct Empty { }
}