using System;
using UnityEngine;
using UnityEngine.UI;

namespace Units.Visual
{
    [ExecuteInEditMode]
    public class BarLife : MonoBehaviour
    {        
        [SerializeField] private Image _imageFill;
        [SerializeField] private Color _color;

        private Unit _unit;

        private void Awake() => _imageFill.color = _color;

        private void OnEnable() => _unit.EntityLife.OnTakeDamage += SetValue;
        private void OnDisable() => _unit.EntityLife.OnTakeDamage -= SetValue;

        private void SetValue(float value)
        {
            _imageFill.fillAmount = value;
        }
        
        #region Builder

        public BarLife SetUnit(Unit unit)
        {
            _unit = unit;
            return this;
        }

        public BarLife SetColor(Color color)
        {
            _imageFill.color = color;
            return this;
        }
        
        #endregion
    }
}