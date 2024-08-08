using EventSystem;
using EventSystem.Channel;
using EventSystem.Listener;
using Player;
using StateMachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Unit : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameObject _selector;
        [SerializeField] private EFaction _faction = EFaction.Blue;

        [Header("Unit Life")] [SerializeField] private EntityLifeSO _entityLifeSO;

        private UnitVisual _unitVisual;
        private EntityLife _unitLife;

        protected FiniteStateMachine fsm;
        protected NavMeshAgent agent;
        protected IDamageable enemyTarget;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            _unitVisual = new UnitVisual(_selector);
            _unitLife = new UnitLife(_entityLifeSO);
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

        public void TakeDamage(int damage)
        {
            _unitLife.TakeDamage(damage);
        }

        public Vector3 GetPosition() => transform.position;
    }
}