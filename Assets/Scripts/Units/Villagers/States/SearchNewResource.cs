using System.Collections.Generic;
using System.Linq;
using Player;
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

        public override void OnExit()
        {
            UnitManager.Instance.RemoveWorkingVillager(villager, villager.GetPreviousResourceType());
        }

        private static void SearchResource(Villager villager)
        {
            if (!IsAnyResource(villager, out Collider[] colliders))
            {
                villager.SetWork(null); // No se encontró para hacer el trabajo.
                return;
            }

            List<IWork> nearWorks = new();

            foreach (Collider col in colliders)
            {
                (bool canAdd, IWork work) resource = CanAddResource(col, villager);

                if (!resource.canAdd) continue;
                nearWorks.Add(resource.work);
            }

            IWork nearWork = nearWorks.OrderBy(x => (x.Position - villager.ActualWork.Position).sqrMagnitude).FirstOrDefault();
            villager.SetWork(nearWork); // Se encontró para seguir trabajando.
        }

        private static bool IsAnyResource(Villager villager, out Collider[] colliders)
        {
            colliders = Physics.OverlapSphere(villager.ActualWork.Position, 7.5f, villager.ActualWork.GetResourceSO().ResourceLayerMask);

            return colliders.Length > 0;
        }

        private static (bool, IWork) CanAddResource(Collider col, Villager villager)
        {
            if (!col.transform.TryGetComponent(out IWork resource)) return (false, resource);
            if (resource.GetResourceSO().ResourceType != villager.ActualWork.GetResourceSO().ResourceType) return (false, resource);
            return !resource.HasResources() ? (false, resource) : (true, resource);
        }
    }
}