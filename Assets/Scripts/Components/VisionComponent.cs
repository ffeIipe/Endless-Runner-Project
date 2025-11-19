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
        private bool _enabled;
        
        private readonly HashSet<Entity> _validTargets;
        private readonly WaitForSeconds _scanInterval;

        public VisionComponent(Enemy owner, EnemyData enemyData, Func<IEnumerator, Coroutine> startCoroutine)
        {
            _owner = owner;
            _enemyData = enemyData;
            _startCoroutine = startCoroutine;
            _ownerTeamType = _owner.GetTeam();

            _enabled = true;
            _validTargets = new HashSet<Entity>();
            _scanInterval = new WaitForSeconds(enemyData.scanInterval);
            
            _startCoroutine(SearchTargets());
        }
        
        public Entity GetTarget()
        {
            _validTargets.RemoveWhere(entity => !entity || !entity.GetAttributesComponent().IsAlive());

            return _validTargets.FirstOrDefault();
        }

        public Vector3 GetTargetDirection()
        {
            var target = GetTarget();
            if (target != null)
            {
                return target.transform.position - _owner.transform.position;
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
            if (_enabled) return;
            _enabled = true;
            _startCoroutine(SearchTargets());
        }

        public void DisableVision()
        {
            _enabled = false;
            _validTargets.Clear();
        }
        
        public bool HasLineOfSight()
        {
            var target = GetTarget();
            if (target == null) return false;

            var startLocation = _owner.transform.position;
            var endLocation = target.transform.position;

            var direction = (endLocation - startLocation).normalized;
            var distance = Vector3.Distance(startLocation, endLocation);

            return !Physics.Raycast(startLocation, direction, distance, _enemyData.obstacleLayer);
        }
        
        private IEnumerator SearchTargets()
        {
            while (_enabled)
            {
                var colliders = Physics.OverlapSphere(_owner.transform.position, _enemyData.attackDistance, _enemyData.targetLayer);
                
                _validTargets.Clear();

                foreach (var col in colliders)
                {
                    if (col == null) continue;

                    var entity = col.GetComponentInParent<Entity>();

                    if (entity != null && 
                        entity.GetTeam() != _ownerTeamType && 
                        entity.GetAttributesComponent().IsAlive())
                    {
                        _validTargets.Add(entity);
                    }
                    else
                    {
                        _validTargets.Remove(entity);
                    }
                }
                
                yield return _scanInterval;
            }
        }
    }
}