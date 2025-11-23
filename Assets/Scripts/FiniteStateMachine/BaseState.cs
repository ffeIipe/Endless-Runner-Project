using Components;
using Entities;
using Entities.Enemies;
using Scriptables;
using UnityEngine.AI;

namespace FiniteStateMachine
{
    public abstract class BaseState
    {
        protected readonly StateMachine StateMachine;
        protected readonly Enemy Owner;
        protected readonly VisionComponent VisionComponent;
        protected readonly EnemyData EnemyData;

        protected BaseState(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
            Owner = StateMachine.Owner;
            
            EnemyData = (EnemyData)StateMachine.Owner.GetData();
            VisionComponent = StateMachine.Owner.GetVisionComponent();
        }

        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();
    }
}
