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
        private readonly Team _ownerTeam;
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;
        private bool _enabled;
        
        private List<Collider> _currentTargets;

        public VisionComponent(Enemy owner, EnemyData enemyData, Func<IEnumerator, Coroutine> startCoroutine)
        {
            _owner = owner;
            _enemyData = enemyData;
            _startCoroutine = startCoroutine;

            _enabled = true;
            _currentTargets = new List<Collider>();
            _startCoroutine(SearchTargets());
            
            _ownerTeam = _owner.GetTeam();
        }
        
        public Collider GetTarget()
        {
            return GetTargets().Count == 0 ? null : GetTargets()[0];
        }

        public Vector3 GetTargetDirection()
        {
            if (GetTarget())
            {
                return GetTarget().transform.position - _owner.transform.position;
            }

            return Vector3.zero;
        }

        private List<Collider> GetTargets()
        {
            if (_currentTargets.Count <= 0) 
            {
                return new List<Collider>(); 
            }
            
            var castedTargets = _currentTargets
                .Where(collider =>
                {
                    if (!collider) return false;

                    var entity = collider.GetComponentInParent<Entity>();
            
                    return entity && entity.GetTeam() != _ownerTeam && entity.GetAttributesComponent().IsAlive();
                })
                .ToList();
    
            return castedTargets;
        }

        public void EnableVision()
        {
            _enabled = true;
            _startCoroutine(SearchTargets());
        }

        public void DisableVision()
        {
            _enabled = false;
        }
        
        public bool HasLineOfSight()
        {
            var startLocation = _owner.transform.position;
            var endLocation = Vector3.zero;
            if (GetTarget())
                endLocation = GetTarget().transform.position;

            var direction = (endLocation - startLocation).normalized;
            var distance = Vector3.Distance(startLocation, endLocation);

            Debug.DrawLine(startLocation, endLocation, Color.magenta, 1f);
            return !Physics.Raycast(startLocation, direction * _enemyData.attackDistance, distance, _enemyData.obstacleLayer);
        }
        
        private IEnumerator SearchTargets()
        {
            while (_enabled)
            {
                if (_currentTargets.Count >= _enemyData.maxTargets) 
                    yield return null;
                
                var results = Physics.OverlapSphere(_owner.transform.position, _enemyData.attackDistance, _enemyData.targetLayer);
                _currentTargets = results.ToList();
                
                yield return null;
            }
        }
    }
}