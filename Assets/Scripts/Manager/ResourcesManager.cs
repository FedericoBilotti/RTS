using UnityEngine;
using Utilities;

namespace Manager
{
    public class ResourcesManager : SingletonAutoGenerated<ResourcesManager>
    {
        public int WoodAmount { get; private set; }
        public int MealAmount { get; private set; }
        public int GoldAmount { get; private set; }
        
        public void AddWoodAmount(int amount)
        {
            WoodAmount += amount;
            Debug.Log("Sumo recurso");
        }

        public void AddMealAmount(int amount)
        {
            MealAmount += amount;
        }

        public void AddGoldAmount(int amount)
        {
            GoldAmount += amount;
        }

        public void SubtractWoodAmount(int amount)
        {
            WoodAmount -= amount;
        }

        public void SubtractMealAmount(int amount)
        {
            MealAmount -= amount;
        }

        public void SubtractGoldAmount(int amount)
        {
            GoldAmount -= amount;
        }
    }
}