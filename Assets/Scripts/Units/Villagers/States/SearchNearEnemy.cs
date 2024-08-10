using System.Linq;
using Manager;
using Units.SO;
using UnityEngine;

namespace Units.Villagers.States
{
    public class SearchNearEnemy : BaseStateVillager
    {
        private readonly VillagerSO _villagerSO;
        public SearchNearEnemy(Villager villager, VillagerSO villagerSO) : base(villager)
        {
            _villagerSO = villagerSO;
        }

        public override void OnEnter()
        {
            SearchNearEnemies(villager, _villagerSO);

            Debug.Log("Busco");

            villager.SetStateName("Search Near Enemy");
        }

        private static void SearchNearEnemies(Villager villager, VillagerSO villagerSO)
        {
            Collider[] enemies = Physics.OverlapSphere(villager.transform.position, villagerSO.SearchNearEnemies, GameManager.Instance.GetUnitAndStructureLayer());

            // Busca enemigos vivos.
            ITargetable[] enemiesAlive = enemies.Select(x => x.GetComponent<ITargetable>()).Where(x =>  !x.IsDead() && x.GetFaction() != villager.GetFaction()).ToArray();

            if (enemiesAlive.Length == 0)
            {
                Debug.Log("No encontró ningun enemigo");
                villager.SetTarget(null);
                return;
            }

            Debug.Log("Encontré un enemigo");
            // Ordena por distancia y obtiene el componente Targeteable.
            ITargetable nearTarget = enemiesAlive.OrderBy(x => (villager.GetPosition() - x.GetPosition()).sqrMagnitude).First();

            villager.SetTarget(nearTarget);
        }
    }
}