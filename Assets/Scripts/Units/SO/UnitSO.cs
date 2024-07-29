using UnityEngine;

namespace Units.SO
{
    public abstract class UnitSO : ScriptableObject
    {
        [Header("Basic stats")]
        [SerializeField] private int _maxLife;
        [SerializeField] private int _damage;
        [SerializeField] private Unit.UnitType _unitType = Unit.UnitType.Villager;
        
        public int MaxLife => _maxLife;
        public int Damage => _damage;
        public Unit.UnitType UnitType => _unitType;
    }
}