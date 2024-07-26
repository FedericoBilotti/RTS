using System.Collections;
using Units.Work;
using UnityEngine;
using Utilities;

namespace Units.Resources
{
    public class Wood : Resource
    {
        public override IWorkable AssignWork() => new WoodWork(this);
        
        public override IEnumerator ProvideResource()
        {
            yield return Helper.GetWaitForSeconds(resource.TimeToGiveResource);
            
            actualAmountOfResource -= resource.AmountToGive;

            if (actualAmountOfResource > 0) yield break;
            
            gameObject.SetActive(false);
        }
    }
}