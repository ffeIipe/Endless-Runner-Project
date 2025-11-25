using Entities;
using Enums;
using Managers;
using UnityEngine;

namespace FiniteStateMachine.States
{
    public class AttackState : BaseState
    {
        protected readonly CountdownTimer CountdownTimer;

        public AttackState(StateMachine stateMachine) : base(stateMachine)
        {
            CountdownTimer = new CountdownTimer(
                Random.Range(
                    EnemyData.minAttackCooldown,
                    EnemyData.maxAttackCooldown
                )
            );
            
            CountdownTimer.OnTimerStop += TryAttack;
        }

        public override void EnterState()
        {
            if (!VisionComponent.GetTarget()) 
                StateMachine.ChangeState("Idle");
            
            CountdownTimer.Start();
        }
        
        public override void ExitState()
        {
            CountdownTimer.Stop();
        }

        public override void UpdateState()
        {
            if (!VisionComponent.GetTarget()) 
                StateMachine.ChangeState("Idle");
            
            FaceTarget();
            CountdownTimer.Tick(Time.deltaTime);
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
                Time.deltaTime * EnemyData.interpSpeed); 
        }
        
        protected virtual void TryAttack()
        {
            ThrowAxe();
            CountdownTimer.Start();
        }

        protected void ThrowAxe()
        {
            var errorAngle = EnemyData.spread; 

            var randomX = Random.Range(-errorAngle, errorAngle);
            var randomY = Random.Range(-errorAngle, errorAngle);
            
            var spreadRotation = Quaternion.Euler(randomX, randomY, 0);
            var dir = VisionComponent.GetTargetDirection().normalized;
            var deviatedDirection = spreadRotation * dir;
            var bullet = FactoryManager.Instance.Spawn<Bullet>(
                PoolableType.Bullet,
                Owner.handPoint.position,
                Owner.handPoint.rotation,
                Owner
            );
            
            bullet.Fire(deviatedDirection, dir);
        }
    }
}
