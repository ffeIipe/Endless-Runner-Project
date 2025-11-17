using FiniteStateMachine.States;
using Pool;
using Scriptables;

namespace Entities.Enemies
{
    public class Viking : Enemy, IPoolable
    {
        private CountdownTimer _deadTimer;
        
        protected override void Awake()
        {
            base.Awake();

            _deadTimer = new CountdownTimer(5f);
            _deadTimer.OnTimerStop += Deactivate;
            
            GetFSM().CreateState("Idle", new IdleState(GetFSM(), GetNavMeshAgent()));
            GetFSM().CreateState("Chase", new ChaseState(GetFSM(), GetNavMeshAgent(), EnemyData));
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
            
            GetNavMeshAgent().enabled = false;
            
            GetFSM().Enabled  = false;
            
            _deadTimer.Start();
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
        }
    }
}