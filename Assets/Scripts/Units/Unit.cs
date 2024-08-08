using Player;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Unit : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameObject _selector;
        [SerializeField] private EFaction _faction = EFaction.Blue;
        
        private UnitVisual _unitVisual;
        
        protected FiniteStateMachine fsm;
        protected NavMeshAgent agent;
        protected IDamageable enemyTarget;

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

            destination = hit.position;
            agent.SetDestination(destination);
        }

        public void SelectUnit() => _unitVisual.SelectUnit();
        public void DeselectUnit() => _unitVisual.DeselectUnit();

        public void SetFaction(EFaction faction) => _faction = faction;
        public EFaction GetFaction() => _faction;

        public void SetEnemyTarget(IDamageable target)
        {
            enemyTarget = target;
            Debug.Log("Target: " + enemyTarget);
        }

        public Vector3 GetPosition() => transform.position;
        public void TakeDamage(int damage) => Debug.Log("Damage");
        
        public void Interact()
        {
            
        }
    }
}