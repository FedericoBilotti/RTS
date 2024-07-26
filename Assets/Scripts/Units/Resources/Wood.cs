using System.Collections;
using Utilities;

namespace Units.Resources
{
    public class Wood : Resource
    {
        public override IEnumerator ProvideResource()
        {
            yield return Helper.GetWaitForSeconds(resource.TimeToGiveResource);
            
            actualAmountOfResource -= GetResourceAmountToGive();

            if (actualAmountOfResource > 0) yield break;
            
            gameObject.SetActive(false);
        }
    }
}