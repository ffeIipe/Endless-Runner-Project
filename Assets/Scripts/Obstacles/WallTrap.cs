using System.Collections;
using Scriptables;
using UnityEngine;

namespace Obstacles
{
    public class WallTrap : MonoBehaviour
    {
        [SerializeField] private WallTrapData wallTrapData;
        
        private Trigger _trigger;
        private Spikes _spikes;
    
        private void Awake()
        {
            _trigger = GetComponentInChildren<Trigger>();
            _trigger.OnTriggered += TriggerTrap;
        
            _spikes = GetComponentInChildren<Spikes>();
            _spikes.SetDamage(wallTrapData.damage);
        }

        private void TriggerTrap()
        {
            StartCoroutine(TriggerSpikes());
        }

        private IEnumerator TriggerSpikes()
        {
            var startPosition = _spikes.transform.localPosition;
            var timer = 0f;
    
            while (timer < wallTrapData.timeTriggerSpike)
            {
                timer += Time.deltaTime;
        
                var percent = timer / wallTrapData.timeTriggerSpike;
                var curveValue = wallTrapData.spikeCurve.Evaluate(percent) * wallTrapData.maxSpikesDistance;
                var offset = Vector3.forward * curveValue;
                _spikes.transform.localPosition = startPosition + offset;
        
                yield return null;
            }

            var finalCurveValue = wallTrapData.spikeCurve.Evaluate(1f) * wallTrapData.maxSpikesDistance;
            _spikes.transform.localPosition = startPosition + (Vector3.forward * finalCurveValue);
        }
    }
}