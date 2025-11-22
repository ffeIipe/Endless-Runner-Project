using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Enemies;
using Enums;
using Scriptables;
using UnityEngine;

namespace Components
{
    public class VisionComponent
    {
        private readonly Enemy _owner;
        private readonly EnemyData _enemyData;
        private readonly TeamType _ownerTeamType;
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;
        private readonly Action<Coroutine> _stopCoroutine;
        
        private Coroutine _searchCoroutine;
        private readonly HashSet<Entity> _validTargets;
        private readonly WaitForSeconds _scanInterval;

        public VisionComponent(Enemy owner, EnemyData enemyData, Func<IEnumerator, Coroutine> startCoroutine, Action<Coroutine> stopCoroutine)
        {
            _owner = owner;
            _enemyData = enemyData;
            _startCoroutine = startCoroutine;
            _stopCoroutine = stopCoroutine;
            _ownerTeamType = _owner.GetTeam();

            _validTargets = new HashSet<Entity>();
            _scanInterval = new WaitForSeconds(enemyData.scanInterval);
        }
        
        public Entity GetTarget()
        {
            _validTargets.RemoveWhere(entity => !entity || !entity.GetAttributesComponent().IsAlive());
            return _validTargets.FirstOrDefault();
        }

        public Vector3 GetTargetDirection()
        {
            var target = GetTarget();
            if (target)
            {
                return target.transform.position - _owner.handPoint.position;
            }

            return Vector3.zero;
        }

        public List<Entity> GetTargetsList()
        {
            _validTargets.RemoveWhere(e => e == null || !e.GetAttributesComponent().IsAlive());
            return _validTargets.ToList();
        }

        public void EnableVision()
        {
            if (_searchCoroutine != null) return;
            _searchCoroutine = _startCoroutine(SearchTargets());
        }

        public void DisableVision()
        {
            if (_searchCoroutine != null)
            {
                _stopCoroutine(_searchCoroutine);
                _searchCoroutine = null;
            }
            _validTargets.Clear();
        }
        
        public bool HasLineOfSight()
        {
            var target = GetTarget();
            if (!target) return false;

            var startLocation = _owner.transform.position + Vector3.up;
            var endLocation = target.transform.position + Vector3.up;

            var direction = (endLocation - startLocation).normalized;
            var distance = Vector3.Distance(startLocation, endLocation);

            return !Physics.Raycast(startLocation, direction, distance, _enemyData.obstacleLayer);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator SearchTargets()
        {
            while (true)
            {
                if (!_owner) yield break;

                var colliders = Physics.OverlapSphere(
                    _owner.transform.position, 
                    _enemyData.attackDistance, 
                    _enemyData.targetLayer,
                    QueryTriggerInteraction.Ignore 
                );
                
                _validTargets.Clear();

                foreach (var col in colliders)
                {
                    if (!col) continue; 

                    var entity = col.GetComponentInParent<Entity>();

                    if (!entity || !entity.GetAttributesComponent().IsAlive()) continue;
                    
                    if (entity.GetTeam() != _ownerTeamType) _validTargets.Add(entity);
                }
        
                yield return _scanInterval;
            }
        }
    }
}