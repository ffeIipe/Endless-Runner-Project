using FiniteStateMachine.States;
using Pool;

namespace Entities.Enemies
{
    public class Viking : Enemy, IPoolable
    {
        protected override void Awake()
        {
            base.Awake();
            
            GetFSM().CreateState("Idle", new IdleState(GetFSM(), GetNavMeshAgent()));
            GetFSM().CreateState("Chase", new ChaseState(GetFSM(), GetNavMeshAgent()));
            GetFSM().CreateState("Attack", new AttackState(GetFSM(), GetNavMeshAgent()));
            
            GetFSM().ChangeState("Idle");
        }

        private void Update()
        {
            GetFSM().Execute();
        }

        protected override void Dead()
        {
            base.Dead();
            Deactivate();
        }
        
        public void Activate()
        {
            gameObject.SetActive(true);
            
            GetNavMeshAgent().enabled = true;
            
            GetFSM().ChangeState("Idle");
            GetFSM().Enabled = true;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            
            //GetNavMeshAgent().isStopped = true;
            GetNavMeshAgent().enabled = false;
            
            GetFSM().ChangeState("Idle");
            GetFSM().Enabled = false;
        }
    }
}