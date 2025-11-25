using FiniteStateMachine.States;

namespace Entities.Enemies
{
    public class Viking : Enemy
    {
        protected override void Awake()
        {
            base.Awake();
            
            GetStateMachine().CreateState("Idle", new IdleState(GetStateMachine()));
            GetStateMachine().CreateState("Attack", CreateAttackState());
            
            GetStateMachine().ChangeState("Idle");
        }

        protected virtual AttackState CreateAttackState()
        {
            return new AttackState(GetStateMachine());
        }
        
        private void Update()
        {
            GetStateMachine().Execute();
        }
    }
}