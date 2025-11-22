using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine.States
{
    public class IdleState : BaseState
    {
        public IdleState(FSM fsm) : base(fsm) { }

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        public override void UpdateState()
        {
            if (VisionComponent.GetTarget())
            {
                FSM.ChangeState("Attack");
            }
        }

        private Vector3 RandomPosition()
        {
            var randomCircle = Random.insideUnitCircle * EnemyData.maxPatrolDistance;
            var randomDirection = new Vector3(randomCircle.x, 0, randomCircle.y);

            var randomPoint = Owner.transform.position + randomDirection;

            var searchRadius = EnemyData.maxPatrolDistance;

            return NavMesh.SamplePosition(randomPoint, out var hit, searchRadius, NavMesh.AllAreas) ? hit.position : Owner.transform.position;
        }
    }
}
