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

        protected FiniteStateMachine fsm;
        protected NavMeshAgent agent;
        protected ITargetable targetable;
        protected EntityLife entityLife;

        public event Action OnSelectUnit = delegate { };
        public event Action OnDeselectUnit = delegate { };

        protected virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            entityLife = GetComponent<EntityLife>();
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

        public void SelectUnit() => OnSelectUnit.Invoke();
        public void DeselectUnit() => OnDeselectUnit.Invoke();

        public void SetFaction(EFaction faction) => _faction = faction;
        public EFaction GetFaction() => _faction;

        public void SetTarget(ITargetable target)
        {
            targetable = target;
        }

        public ITargetable GetTarget() => targetable;
        public Vector3 GetPosition() => transform.position;
        public EntityLife GetEntity() => entityLife;

        public void TakeDamage(int damage) => entityLife.TakeDamage(damage);
        public bool IsDead() => entityLife.IsDead();
    }
}