using System.Collections;
using System.Linq;
using Manager;
using Units.Resources;
using UnityEngine;

namespace Units.Work
{
    public class WoodWork : IWorkable
    {
        public bool IsWorking { get; private set; }

        private Resource _resource;

        public WoodWork(Resource resource) => _resource = resource;

        public IEnumerator Working(Unit unit)
        {
            bool savingResource = false;
            Transform centerTransform = GameManager.Instance.NearCenter(unit).transform;
            Transform desiredTransform = _resource.transform;

            int amountToGive = _resource.GetResourceAmountToGive();

            while (true)
            {
                float distance = (desiredTransform.position - unit.transform.position).magnitude;

                if (distance > 2f)
                {
                    unit.SetDestination(savingResource ? centerTransform.position : _resource.transform.position);
                    yield return null;
                }
                else if (savingResource)
                {
                    if (distance > centerTransform.localScale.x * 0.5f) yield return null;

                    savingResource = false;
                    desiredTransform = _resource.transform;
                    ResourcesManager.Instance.AddWoodAmount(amountToGive);

                    LookTree(unit);
                }
                else
                {
                    unit.StopMovement();
                    
                    // Hacer animación
                    IsWorking = true;
                    yield return unit.StartCoroutine(_resource.ProvideResource());
                    IsWorking = false;
                    
                    savingResource = true;
                    centerTransform = GameManager.Instance.NearCenter(unit).transform;
                    desiredTransform = centerTransform;
                }
            }
        }

        private void LookTree(Unit unit)
        {
            if (_resource.gameObject.activeSelf) return;

            SearchNewTree(unit, out _resource);
                    
            if (_resource == null) unit.StopAllCoroutines();
        }

        private static void SearchNewTree(Unit unit, out Resource resource)
        {
             Collider[] col = Physics.OverlapSphere(unit.transform.position, 30f, GameManager.Instance.GetTreesLayer());

            if (col.Length == 0)
            {
                unit.StopAllCoroutines();
                resource = null;
                return;
            }

            Collider near = col.OrderBy(tree => (tree.transform.position - unit.transform.position).sqrMagnitude).First();
            
            unit.SetDestination(near.transform.position);
            resource = near.GetComponent<Resource>();
        }
    }
}