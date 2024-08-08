using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "EntityLifeSO", menuName = "Units/UnitLifeSO")]
    public class EntityLifeSO : ScriptableObject
    {
        [SerializeField] private int _maxLife;

        public int MaxLife => _maxLife;

        /// <summary>
        /// Obtiene la vida actual y la divide con la vida máxima
        /// </summary>
        /// <param name="actualLife">La vida actual de la entidad</param>
        /// <returns>Devuelve un valor entre 0 y 1</returns>
        public float CalculateLifePercentage(float actualLife)
        {
            return actualLife / _maxLife;
        }
    }
}