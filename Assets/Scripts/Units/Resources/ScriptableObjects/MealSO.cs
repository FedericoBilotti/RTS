using UnityEngine;

namespace Units.Resources.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MealSO", menuName = "Resources/Meal")]
    public class MealSO : ResourceSO
    {
        [SerializeField] private int _mealAmount = 200;

        public int MealAmount => _mealAmount;
    }
}