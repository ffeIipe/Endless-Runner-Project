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
            _countdownTimer = new CountdownTimer(Random.Range(EnemyData.minAttackCooldown, EnemyData.minAttackCooldown * EnemyData.maxAttackCooldown));
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
                Time.deltaTime * EnemyData.interpSpeed); 
        }
        
        private void TryAttack()
        {
            if (!VisionComponent.GetTarget()) return;
            
            ThrowAxe();
            
            _countdownTimer.Start();
        }

        private void ThrowAxe()
        {
            var errorAngle = EnemyData.spread; 

            var randomX = Random.Range(-errorAngle, errorAngle);
            var randomY = Random.Range(-errorAngle, errorAngle);

            var spreadRotation = Quaternion.Euler(randomX, randomY, 0);

            var deviatedDirection = spreadRotation * Owner.handPoint.forward;

            var deviatedRotation = Quaternion.LookRotation(deviatedDirection);

            var bullet = BulletFactory.Instance.SpawnBullet(Owner.transform, Owner);
    
            bullet.Fire(deviatedDirection, deviatedRotation, Vector3.one);
        }
    }
}
