using System.Linq;
using Manager;
using Units.SO;
using UnityEngine;
using UnityEngine.AI;

namespace Units.Jedi
{
    public class SearchNearEnemy : BaseStateJedi
    {
        public SearchNearEnemy(Jedi jedi, NavMeshAgent agent, JediSO jediSO) : base(jedi, agent, jediSO) { }

        public override void OnEnter()
        {
            SearchNearEnemies(jedi, jediSO);
        }

        private static void SearchNearEnemies(Jedi jedi, JediSO jediSO)
        {
            Collider[] enemies = Physics.OverlapSphere(jedi.transform.position, jediSO.SearchNearEnemies, GameManager.Instance.GetUnitAndStructureLayer());

            if (enemies.Length == 0) return;
            
            // Ordena por distancia y obtiene el componente Targeteable.
            ITargetable nearTarget = enemies.OrderBy(x => (jedi.GetPosition() - x.transform.position).sqrMagnitude).First().GetComponent<ITargetable>();
            
            jedi.SetTarget(nearTarget);
        }
    }
}