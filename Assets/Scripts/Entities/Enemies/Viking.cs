using FiniteStateMachine.States;

namespace Entities.Enemies
{
    public class Viking : Enemy
    {
        protected override void Awake()
        {
            base.Awake();
            
            GetStateMachine().CreateState("Idle", new IdleState(GetStateMachine()));
            GetStateMachine().CreateState("Attack", new AttackState(GetStateMachine()));
            
            GetStateMachine().ChangeState("Idle");
        }

        private void Update()
        {
            GetStateMachine().Execute();
        }
    }
}