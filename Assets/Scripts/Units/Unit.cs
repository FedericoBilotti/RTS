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
        private UnitVisual _unitVisual;

        protected NavMeshAgent agent;
        protected FiniteStateMachine fsm;

        [SerializeField] public EFaction _faction = EFaction.Blue;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            _unitVisual = new UnitVisual(_selector);
        }

        public void StopMovement() => agent.isStopped = true;

        public void SetDestination(Vector3 destination)
        {
            agent.isStopped = false;

            // Make sure the position is valid.
            if (!NavMesh.SamplePosition(destination, out NavMeshHit hit, 5f, NavMesh.AllAreas)) return;

            agent.SetDestination(hit.position);
        }

        public void SelectUnit() => _unitVisual.SelectUnit();
        public void DeselectUnit() => _unitVisual.DeselectUnit();

        public void SetFaction(EFaction faction) => _faction = faction;
        public EFaction GetFaction() => _faction;

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
}