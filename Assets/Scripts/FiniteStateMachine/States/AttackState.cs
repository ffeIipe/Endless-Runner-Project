using Factories;
using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine.States
{
    public class AttackState : BaseState
    {
        private CountdownTimer _countdownTimer;

        public AttackState(FSM fsm) : base(fsm)
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

            var dir = (VisionComponent.GetTarget().transform.position - Owner.transform.position).normalized;
            
            var targetRotation = Quaternion.LookRotation(dir);
            targetRotation.x = 0;
            targetRotation.z = 0;
            
            Owner.transform.rotation = Quaternion.Slerp(
                Owner.transform.rotation, 
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

            ThrowAxe();
            
            _countdownTimer.Start();
        }

        private void ThrowAxe()
        {
            var bullet = BulletFactory.Instance.SpawnBullet(Owner.transform, Owner);
            bullet.Fire(Owner.handPoint.forward, Owner.transform.rotation, Vector3.one);
        }
    }
}
