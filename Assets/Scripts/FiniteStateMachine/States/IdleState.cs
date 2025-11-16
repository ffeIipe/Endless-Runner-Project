using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine.States
{
    public class IdleState : BaseState
    {
        public IdleState(FSM fsm, NavMeshAgent agent) : base(fsm, agent) { }

        public override void EnterState()
        {
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
        }
    }
}
