using EventSystem.Channel;
using UnityEngine;
using UnityEngine.Events;

namespace EventSystem.Listener
{
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] private EventChannel<T> _eventChannel;
        [SerializeField] private UnityEvent<T> _unityEvent;

        private void OnEnable() => _eventChannel.RegisterListener(this);
        private void OnDisable() => _eventChannel.UnregisterListener(this);

        public void Raise(T value)
        {
            _unityEvent.Invoke(value);
        }

        public void AddListener(UnityAction<T> value)
        {
            _unityEvent.AddListener(value);
        }

        public void RemoveListener(UnityAction<T> value)
        {
            _unityEvent.RemoveListener(value);
        }
    }

    public class EventListener : EventListener<Empty> { }
}