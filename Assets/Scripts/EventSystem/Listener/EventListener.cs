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
    }
    
    public class EventListener : EventListener<Empty>
    {
    }
}