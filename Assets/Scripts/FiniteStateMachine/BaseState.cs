using Components;
using Entities;
using Entities.Enemies;
using Scriptables;
using UnityEngine.AI;

namespace FiniteStateMachine
{
    public abstract class BaseState
    {
        protected readonly FSM FSM;
        protected readonly Enemy Owner;
        protected readonly VisionComponent VisionComponent;
        protected readonly EnemyData EnemyData;

        protected BaseState(FSM fsm)
        {
            FSM = fsm;
            Owner = FSM.Owner;
            
            EnemyData = (EnemyData)FSM.Owner.GetData();
            VisionComponent = FSM.Owner.GetVisionComponent();
        }

        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();
    }
}
