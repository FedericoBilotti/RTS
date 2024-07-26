using Manager;
using StateMachine;
using Units.Resources;
using Units.Villagers.States;

namespace Units.Villagers
{
    public class Villager : Unit
    {
        private Resource _actualResource;
        private int _actualAmountResource;
        
        private void Start() => CreateFSM();

        private void Update() => fsm?.Update();
        private void FixedUpdate() => fsm?.FixedUpdate();

        private void CreateFSM()
        {
            fsm = new FiniteStateMachine();

            var idle = new Idle();
            var mining = new Mining(this);
            var chopping = new Chop(this);  
            var moveToResource = new MoveToResource(this);
            var moveToCenter = new MoveToStorage(this);
            
            // from idle
            fsm.AddTransition(idle, moveToResource, new FuncPredicate(() => _actualResource != null && _actualResource.GetActualAmount() > 0));
            
            // from moveToResource
            fsm.AddTransition(moveToResource, chopping, new FuncPredicate(MoveToChopping)); 
            fsm.AddTransition(moveToResource, mining, new FuncPredicate(MoveToMining));
            
            // from mining
            fsm.AddTransition(mining, moveToCenter, new FuncPredicate(() => _actualAmountResource > 0));
            
            // from chop
            fsm.AddTransition(chopping, moveToCenter, new FuncPredicate(() => _actualAmountResource > 0));
            
            // from moveToCenter
            fsm.AddTransition(moveToCenter, moveToResource, new FuncPredicate(() => _actualResource != null && _actualResource.GetActualAmount() > 0 && _actualAmountResource <= 0));
            
            fsm.SetState(idle);
        }

        private bool MoveToChopping() => _actualResource.ResourceType() == ResourcesManager.ResourceType.Gold && (transform.position - _actualResource.transform.position).sqrMagnitude < 3f * 3f;
        private bool MoveToMining() => _actualResource.ResourceType() == ResourcesManager.ResourceType.Gold && (transform.position - _actualResource.transform.position).sqrMagnitude < 3f * 3f;

        public void SetResource(Resource resource) => _actualResource = resource;
        public Resource GetResource() => _actualResource;

        public void SetAmountResource(int amount) => _actualAmountResource = amount;
    }
}