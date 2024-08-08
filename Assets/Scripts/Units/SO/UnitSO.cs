using UnityEngine;

namespace Units.SO
{
    public abstract class UnitSO : ScriptableObject
    {
        [Header("Basic stats")]
        [SerializeField] private int _maxLife;
        [SerializeField] private int _damage;
        [SerializeField] private UnitType _unitType = UnitType.Villager;
        
        [Header("NavMesh")]
        [SerializeField] private int _stoppingDistanceToIdle = 0;
        [SerializeField] private int _stoppingDistanceToAttack = 1;
        
        public int MaxLife => _maxLife;
        public int Damage => _damage;
        public UnitType UnitType => _unitType;
        
        public int StoppingDistanceToIdle => _stoppingDistanceToIdle;
        public int StoppingDistanceToAttack => _stoppingDistanceToAttack;
    }
}