using UnityEngine.AI;

namespace FiniteStateMachine
{
    public abstract class BaseState
    {
        public static bool isIdle;
        public static bool isPatrol;
        public static bool isAttacking;
        public static bool isTeleporting = false;

        protected FSM FSM;
        protected NavMeshAgent Agent;

        public BaseState(FSM fsm,  NavMeshAgent agent)
        {
            FSM = fsm;
            Agent = agent;
        }

        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();
    }
}
