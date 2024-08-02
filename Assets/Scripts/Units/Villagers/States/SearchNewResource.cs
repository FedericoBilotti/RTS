using System.Collections.Generic;
using System.Linq;
using Units.Resources;
using UnityEngine;

namespace Units.Villagers.States
{
    public class SearchNewResource : BaseStateVillager
    {
        public SearchNewResource(Villager villager) : base(villager) { }

        public override void OnEnter()
        {
            SearchResource(villager);

            villager.SetStateName("Search new resource");
        }

        private static void SearchResource(Villager villager)
        {
            if (!IsAnyResource(villager, out Collider[] colliders))
            {
                villager.SetWork(null); // No se encontró para hacer el trabajo.
                return;
            }

            List<IWork> resources = new();

            foreach (Collider col in colliders)
            {
                var canAddResource = CanAddResource(col, villager);
                
                if (!canAddResource.Item1) continue;
                resources.Add(canAddResource.Item2);
            }

            IWork newResource = resources.OrderBy(x => (x.Position - villager.ActualWork.Position).sqrMagnitude).FirstOrDefault();
            villager.SetWork(newResource); // Se encontró para seguir trabajando.
        }

        private static bool IsAnyResource(Villager villager, out Collider[] colliders)
        {
            colliders = Physics.OverlapSphere(villager.ActualWork.Position, 10f);

            return colliders.Length > 0;
        }

        private static (bool, IWork) CanAddResource(Collider col, Villager villager)
        {
            if (!col.transform.TryGetComponent(out IWork resource)) return (false, resource);
            if (resource.GetResourceSO().ResourceType != villager.ActualWork.GetResourceSO().ResourceType) return (false, resource);
            return resource.GetActualAmount() <= 0 ? (false, resource) : (true, resource);
        }
    }
}