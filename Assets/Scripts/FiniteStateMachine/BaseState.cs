using Components;
using Entities;
using Scriptables;
using UnityEngine.AI;

namespace FiniteStateMachine
{
    public abstract class BaseState
    {
        protected readonly FSM FSM;
        protected readonly NavMeshAgent Agent;
        protected readonly VisionComponent VisionComponent;
        protected readonly EnemyData EnemyData;

        protected BaseState(FSM fsm, NavMeshAgent agent)
        {
            FSM = fsm;
            Agent = agent;
            
            EnemyData = (EnemyData)FSM.Owner.GetData();
            VisionComponent = FSM.Owner.GetVisionComponent();
        }

        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();
    }
}
