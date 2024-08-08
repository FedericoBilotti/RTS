using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "EntityLifeSO", menuName = "Units/UnitLifeSO")]
    public class EntityLifeSO : ScriptableObject
    {
        [SerializeField] private int _maxLife;

        public int MaxLife => _maxLife;

        public float PercentageLife(float actualLife)
        {
            return actualLife / _maxLife;
        }
    }
}