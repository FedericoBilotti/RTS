using System.Collections.Generic;
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

            Debug.Log("Busco");

            jedi.SetStateName("Search Near Enemy");
        }

        private static void SearchNearEnemies(Jedi jedi, JediSO jediSO)
        {
            Collider[] enemies = Physics.OverlapSphere(jedi.transform.position, jediSO.SearchNearEnemies, GameManager.Instance.GetUnitAndStructureLayer());

            // Busca enemigos vivos.
            ITargetable[] enemiesAlive = enemies.Select(x => x.GetComponent<ITargetable>()).Where(x =>  !x.IsDead() && x.GetFaction() != jedi.GetFaction()).ToArray();

            if (enemiesAlive.Length == 0)
            {
                Debug.Log("No encontró ningun enemigo");
                jedi.SetTarget(null);
                return;
            }

            Debug.Log("Encontré un enemigo");
            // Ordena por distancia y obtiene el componente Targeteable.
            ITargetable nearTarget = enemiesAlive.OrderBy(x => (jedi.GetPosition() - x.GetPosition()).sqrMagnitude).First();

            jedi.SetTarget(nearTarget);
        }
    }
}