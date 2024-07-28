using Units.Resources;
using UnityEngine;

namespace Units.Villagers.States
{
    public class SearchNewResource : BaseState
    {
        private readonly Villager _villager;

        public SearchNewResource(Villager villager) => _villager = villager;

        public override void OnEnter()
        {
            SearchResource(_villager);
            
            _villager.SetName("Search new resource");
        }

        private static void SearchResource(Villager villager)
        {
            Collider[] resources = Physics.OverlapSphere(villager.GetResourceTransform().position, 10f);
            
            if (resources.Length == 0) return;

            foreach (Collider col in resources)
            {
                if (!col.transform.TryGetComponent(out Resource resource)) continue;
                if (resource.GetResourceType() != villager.GetResourceType()) continue;
                if (resource.GetActualAmount() <= 0) continue;
                
                villager.SetResource(resource);
                break;
            }
        } 
    }
}