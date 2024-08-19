using System;
using UnityEngine;
using UnityEngine.UI;

namespace Units.Visual
{
    public class BarLife : MonoBehaviour
    {
        [SerializeField] private Image _imageFill;
        public Transform ChildTransform { get; private set; }

        private IDamageable _damageable;

        private void Awake()
        {
            ChildTransform = transform.GetChild(0);
        }

        private void Start()
        {
            _damageable.GetEntity().OnTakeDamage += SetValue;
            ChildTransform.gameObject.SetActive(false);
        }

        private void OnDestroy() => _damageable.GetEntity().OnTakeDamage -= SetValue;

        private void SetValue(float value)
        {
            _imageFill.fillAmount = value;
        }

        private void Update()
        {
            Vector3 entityPosition = _damageable.GetEntity().transform.position;
            ChildTransform.position = entityPosition + Vector3.back;
        }

        public BarLife SetDamageable(IDamageable damageable)
        {
            _damageable = damageable;
            return this;
        }
    }
}