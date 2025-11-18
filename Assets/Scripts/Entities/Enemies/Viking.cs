using FiniteStateMachine.States;
using Pool;

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
            
            GetFSM().CreateState("Idle", new IdleState(GetFSM()));
            GetFSM().CreateState("Chase", new ChaseState(GetFSM()));
            GetFSM().CreateState("Attack", new AttackState(GetFSM()));
            
            GetFSM().ChangeState("Idle");
        }

        private void Update()
        {
            GetFSM().Execute();
        }

        protected override void Dead()
        {
            base.Dead();
            
            GetFSM().Enabled  = false;
            
            _deadTimer.Start();
        }
        
        public void Activate()
        {
            RemoveDissolveEffect();
            GetRigidbody().constraints = SavedRigidbodyConstraints;
            gameObject.SetActive(true);
            
            GetFSM().ChangeState("Idle");
            GetFSM().Enabled = true;
        }

        public void Deactivate()
        {
            ApplyDissolveEffect();
        }
    }
}