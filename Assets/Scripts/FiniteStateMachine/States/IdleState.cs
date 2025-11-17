using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine.States
{
    public class IdleState : BaseState
    {
        public IdleState(FSM fsm, NavMeshAgent agent) : base(fsm, agent) { }

        public override void EnterState()
        {
            Agent.SetDestination(RandomPosition());
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
            else
            {
                Debug.Log("Invalid target");
            }
        }

        private Vector3 RandomPosition()
        {
            var randomCircle = Random.insideUnitCircle * EnemyData.maxPatrolDistance;
            var randomDirection = new Vector3(randomCircle.x, 0, randomCircle.y);

            var randomPoint = Agent.transform.position + randomDirection;

            var searchRadius = EnemyData.maxPatrolDistance;

            return NavMesh.SamplePosition(randomPoint, out var hit, searchRadius, NavMesh.AllAreas) ? hit.position : Agent.transform.position;
        }
    }
}
