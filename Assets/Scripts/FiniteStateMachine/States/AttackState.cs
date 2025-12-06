using System;
using Entities;
using Enums;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FiniteStateMachine.States
{
    public class AttackState : BaseState
    {
        protected readonly CountdownTimer AttackTimer;
        private Action _onAxeThrown = delegate { };

        public AttackState(StateMachine stateMachine) : base(stateMachine)
        {
            AttackTimer = new CountdownTimer(EnemyData.attackCooldown);
        }

        public override void EnterState()
        {
            if (!VisionComponent.GetTarget()) 
                StateMachine.ChangeState("Idle");

            _onAxeThrown += Owner.GetView().NotAttackEffect;
            
            AttackTimer.OnTimerStart += Owner.GetView().AttackEffect;
            AttackTimer.OnTimerStop += TryAttack;
            
            AttackTimer.Start();
        }
        
        public override void ExitState()
        {
            _onAxeThrown -= Owner.GetView().NotAttackEffect;
            
            AttackTimer.OnTimerStart -= Owner.GetView().AttackEffect;
            AttackTimer.OnTimerStop -= TryAttack;
            
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
            
            _onAxeThrown?.Invoke();
            bullet.Fire(deviatedDirection, dir);
        }
    }
}
