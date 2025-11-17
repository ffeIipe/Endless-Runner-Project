using Factories;
using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine.States
{
    public class AttackState : BaseState
    {
        private CountdownTimer _countdownTimer;

        public AttackState(FSM fsm, NavMeshAgent agent) : base(fsm, agent)
        {
            _countdownTimer = new CountdownTimer(Random.Range(EnemyData.attackCooldown, EnemyData.attackCooldown * 1.5f));
            _countdownTimer.OnTimerStop += TryAttack;
        }

        public override void EnterState()
        {
            if (!VisionComponent.GetTarget()) FSM.ChangeState("Idle");
            
            _countdownTimer.Start();
        }


        public override void ExitState()
        {
            _countdownTimer.Stop();
        }

        public override void UpdateState()
        {
            if (!VisionComponent.GetTarget()) FSM.ChangeState("Idle");
            
            FaceTarget();
            _countdownTimer.Tick(Time.deltaTime);
        }

        private void FaceTarget()
        {
            if (!VisionComponent.GetTarget()) return;

            var dir = (VisionComponent.GetTarget().transform.position - Agent.transform.position).normalized;
            
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var targetRotation = Quaternion.Euler(0, 0, angle - 90f);
            
            Agent.transform.rotation = Quaternion.Slerp(
                Agent.transform.rotation, 
                targetRotation, 
                Time.deltaTime * 10f); 
        }
        
        private void TryAttack()
        {
            if (!VisionComponent.GetTarget())
            {
                _countdownTimer.Start();
                return;
            }

            BulletFactory.Instance.SpawnBullet(Agent.transform, FSM.Owner);
            
            _countdownTimer.Start();
        }
    }
}
