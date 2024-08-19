using Pool.LifeBar;
using Units.Visual;
using UnityEngine;

namespace Units
{
    public class ConsumeBarLife : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private BarLife _barLife;

        private void Start()
        {
            _barLife = BarLifePool.Instance.CreateObject().SetDamageable(_unit);
        }

        private void OnEnable()
        {
            _unit.OnSelectUnit += TurnOnBarLife;
            _unit.OnDeselectUnit += TurnOffBarLife;
        }

        private void OnDisable()
        {
            _unit.OnSelectUnit -= TurnOnBarLife;
            _unit.OnDeselectUnit -= TurnOffBarLife;
        }

        private void TurnOnBarLife() => _barLife.ChildTransform.gameObject.SetActive(true);
        private void TurnOffBarLife() => _barLife.ChildTransform.gameObject.SetActive(false);
    }
}