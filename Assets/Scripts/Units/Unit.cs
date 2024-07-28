using Player;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] private GameObject _selector;
        [SerializeField] private UnitType _unitType;
        protected NavMeshAgent agent;

        public UnitVisual UnitVisual { get; private set; }
        protected FiniteStateMachine fsm;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            UnitVisual = new UnitVisual(_selector);
        }

        private void OnEnable() => UnitManager.Instance.AddAvailableUnit(this);
        private void OnDisable() => UnitManager.Instance.RemoveAvailableUnit(this);

        public void StopMovement() => agent.isStopped = true;
        
        public void SetDestination(Vector3 destination)
        {
            agent.isStopped = false;
            agent.SetDestination(destination);
        }

        public UnitType GetUnitType() => _unitType;
    }

    public enum UnitType
    {
        None,
        Villager,
        Padawan,
        Jedi,
        JediMaster,
        StormTrooper,
    }
}