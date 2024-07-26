using UnityEngine;

namespace Units.Resources.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GoldSO", menuName = "Resources/Gold")]
    public class GoldSO : ResourceSO
    {
        [SerializeField] private int _goldAmount = 2000;

        public int GoldAmount => _goldAmount;
    }
}