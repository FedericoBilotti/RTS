using System.Security.Cryptography;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] private GameObject _selector;
        protected NavMeshAgent agent;

        public UnitVisual UnitVisual { get; private set; }
        protected FiniteStateMachine fsm;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            UnitVisual = new UnitVisual(_selector);
        }

        public void StopMovement() => agent.isStopped = true;

        public void SetDestination(Vector3 destination)
        {
            agent.isStopped = false;

            agent.SetDestination(destination);
            
            // Make sure the position is valid.
            if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 5f, NavMesh.AllAreas)) { }
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
}