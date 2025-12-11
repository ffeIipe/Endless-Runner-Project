using System;
using Enums;
using Managers;
using Obstacles;
using UnityEngine;

namespace Scriptables.Obstacles
{
    [RequireComponent(typeof(Trigger))]
    public class SignTrigger : MonoBehaviour
    {
        private Trigger _trigger;
        
        private void Awake()
        {
            _trigger = GetComponent<Trigger>();
        }

        private void OnEnable()
        {
            _trigger.OnTriggered += OnTriggered;
        }

        private void OnDisable()
        {
            _trigger.OnTriggered -= OnTriggered;
        }

        private void OnTriggered()
        {
            EffectsManager.Instance.PlayEffect(TimeWarpType.Medium);
        }
    }
}
