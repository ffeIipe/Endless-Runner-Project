using Entities;
using Enums;
using Managers;
using UnityEngine;

namespace FiniteStateMachine.States
{
    public class AttackState : BaseState
    {
        protected readonly CountdownTimer AttackTimer;

        public AttackState(StateMachine stateMachine) : base(stateMachine)
        {
            AttackTimer = new CountdownTimer(
                Random.Range(
                    EnemyData.minAttackCooldown,
                    EnemyData.maxAttackCooldown
                )
            );
            
            AttackTimer.OnTimerStop += TryAttack;
        }

        public override void EnterState()
        {
            if (!VisionComponent.GetTarget()) 
                StateMachine.ChangeState("Idle");
            
            AttackTimer.Start();
        }
        
        public override void ExitState()
        {
            AttackTimer.Stop();
        }

        public override void UpdateState()
        {
            if (!VisionComponent.GetTarget()) 
                StateMachine.ChangeState("Idle");
            
            FaceTarget();
            AttackTimer.Tick(Time.deltaTime);
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
            AttackTimer.Start();
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
