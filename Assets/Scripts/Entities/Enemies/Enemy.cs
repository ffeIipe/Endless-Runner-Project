using FiniteStateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Enemy : Entity
    {
        private FSM _fsm;
        private NavMeshAgent _agent;

        protected override void Awake()
        {
            base.Awake();
            
            _fsm = new FSM();
            _agent = GetComponent<NavMeshAgent>();
        }

        protected FSM GetFSM() => _fsm;
        protected NavMeshAgent GetNavMeshAgent() => _agent;
    }
}