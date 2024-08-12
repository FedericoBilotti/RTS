using System;
using Player;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    [RequireComponent(typeof(NavMeshAgent), typeof(UnitLife), typeof(UnitVisual))]
    public abstract class Unit : MonoBehaviour, ITargetable
    {
        [SerializeField] private EFaction _faction = EFaction.Blue;

        private EntityLife _entityLife;

        public Action onSelectUnit = delegate { };
        public Action onDeselectUnit = delegate { };

        protected FiniteStateMachine fsm;
        protected NavMeshAgent agent;
        protected ITargetable targetable;

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            _entityLife = GetComponent<EntityLife>();
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();

        public void StopMovement() => agent.isStopped = true;

        public void SetDestination(Vector3 destination)
        {
            agent.isStopped = false;

            // Make sure the position is valid.
            if (!NavMesh.SamplePosition(destination, out NavMeshHit hit, 5f, NavMesh.AllAreas)) return;

            destination = hit.position;
            agent.SetDestination(destination);
        }

        public void SelectUnit() => onSelectUnit.Invoke();
        public void DeselectUnit() => onDeselectUnit.Invoke();

        public void SetFaction(EFaction faction) => _faction = faction;
        public EFaction GetFaction() => _faction;

        public void SetTarget(ITargetable target)
        {
            targetable = target;
        }

        public ITargetable GetTarget() => targetable;
        public Vector3 GetPosition() => transform.position;
        public void TakeDamage(int damage) => _entityLife.TakeDamage(damage);
        public bool IsDead() => _entityLife.IsDead();
    }
}