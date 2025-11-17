using Components;
using FiniteStateMachine;
using Scriptables;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Enemy : Entity
    {
        protected EnemyData EnemyData;
        private FSM _fsm;
        private NavMeshAgent _agent;
        private VisionComponent _visionComponent;

        protected override void Awake()
        {
            base.Awake();
            
            EnemyData = (EnemyData)entityData;
            
            _visionComponent = new VisionComponent(this, EnemyData, StartCoroutine);
            _fsm = new FSM(this);
            _agent = GetComponent<NavMeshAgent>();
        }

        public VisionComponent GetVisionComponent() => _visionComponent;
        protected FSM GetFSM() => _fsm;
        public NavMeshAgent GetNavMeshAgent() => _agent;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, EnemyData.attackDistance);
        }

        public EntityData GetData() => EnemyData;
    }
}