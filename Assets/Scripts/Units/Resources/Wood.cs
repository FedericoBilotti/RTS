using System.Collections;
using Units.Work;
using UnityEngine;
using Utilities;

namespace Units.Resources
{
    public class Wood : Resource
    {
        public override UnitType UnitDesired() => UnitType.Villager;
        public override IWorkable AssignWork() => new WoodWork(this);
        
        public override IEnumerator ProvideResource()
        {
            yield return Helper.GetWaitForSeconds(resource.TimeToGiveResource);
            
            Debug.Log("Giving resource");
            actualAmountOfResource -= resource.AmountToGive;

            if (actualAmountOfResource <= 0)
            {
                Debug.Log("Turn Off");
                gameObject.SetActive(false);
            }
        }
    }
}