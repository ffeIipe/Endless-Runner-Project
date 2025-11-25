using System.Collections;
using Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace FiniteStateMachine.States
{
    public class JumpAttackState : AttackState
    {
        public JumpAttackState(StateMachine stateMachine) : base(stateMachine) { }
        
        protected override void TryAttack()
        {
            Owner.StartCoroutine(JumpAttack());
            CountdownTimer.Start();
        }

        private IEnumerator JumpAttack()
        {
            var timer = 0f;
            var lastCurveValue = 0f;
            var hasAttacked = false;

            while (timer < EnemyData.jumpDuration)
            {
                if (!GameManager.IsPaused) 
                {
                    timer += Time.deltaTime;

                    if (timer > EnemyData.jumpDuration / 2 && !hasAttacked)
                    {
                        hasAttacked = true;
                        ThrowAxe();
                    }
                    
                    var percent = timer / EnemyData.jumpDuration;
                    var curveValue = EnemyData.jumpCurve.Evaluate(percent) * EnemyData.jumpHeight;
                    var moveDeltaY = (curveValue - lastCurveValue);
                    var moveVector = new Vector3(0, moveDeltaY, 0);

                    Owner.transform.position += moveVector;

                    lastCurveValue = curveValue;
                }
                
                yield return null;
            }
        }
    }
}
