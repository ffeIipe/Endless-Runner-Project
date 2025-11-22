using FiniteStateMachine.States;

namespace Entities.Enemies
{
    public class Viking : Enemy
    {
        protected override void Awake()
        {
            base.Awake();
            
            GetFSM().CreateState("Idle", new IdleState(GetFSM()));
            GetFSM().CreateState("Attack", new AttackState(GetFSM()));
            
            GetFSM().ChangeState("Idle");
        }

        private void Update()
        {
            GetFSM().Execute();
        }
    }
}