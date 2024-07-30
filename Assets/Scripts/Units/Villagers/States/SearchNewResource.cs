using System.Collections.Generic;
using System.Linq;
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

            _villager.SetStateName("Search new resource");
        }

        private static void SearchResource(Villager villager)
        {
            if (!IsAnyResource(villager, out Collider[] colliders))
            {
                villager.SetResource(null);
                return;
            }

            List<Resource> resources = new();

            foreach (Collider col in colliders)
            {
                var canAddResource = CanAddResource(col, villager);
                
                if (!canAddResource.Item1) continue;
                resources.Add(canAddResource.Item2);
            }

            Resource newResource = resources.OrderBy(x => (x.transform.position - villager.GetResourceTransform().position).sqrMagnitude).FirstOrDefault();
            villager.SetResource(newResource);
        }

        private static bool IsAnyResource(Villager villager, out Collider[] colliders)
        {
            colliders = Physics.OverlapSphere(villager.GetResourceTransform().position, 10f);

            return colliders.Length > 0;
        }

        private static (bool, Resource) CanAddResource(Collider col, Villager villager)
        {
            if (!col.transform.TryGetComponent(out Resource resource)) return (false, resource);
            if (resource.GetResourceType() != villager.GetResourceType()) return (false, resource);
            return resource.GetActualAmount() <= 0 ? (false, resource) : (true, resource);
        }
    }
}