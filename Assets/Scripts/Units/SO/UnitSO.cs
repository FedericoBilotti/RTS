using UnityEngine;

namespace Units.SO
{
    public abstract class UnitSO : ScriptableObject
    {
        [Header("Basic stats")]
        [SerializeField] private int _maxLife;
        [SerializeField] private int _damage;
        [SerializeField] private UnitType _unitType = UnitType.Villager;

        [Header("Attack")]
        [SerializeField] private float _searchNearEnemies = 10f;
        
        [Header("NavMesh")]
        [SerializeField] private float _stoppingDistanceToIdle = 0;
        [SerializeField] private float _stoppingDistanceToAttack = 2;
        
        public int MaxLife => _maxLife;
        public int Damage => _damage;
        public UnitType UnitType => _unitType;
        
        public float SearchNearEnemies => _searchNearEnemies;
        
        public float StoppingDistanceToIdle => _stoppingDistanceToIdle;
        public float StoppingDistanceToAttack => _stoppingDistanceToAttack;
    }
}