using System;
using UnityEngine;

namespace Obstacles
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class Trigger : MonoBehaviour
    {
        [SerializeField] private string triggerName; 
        private BoxCollider _boxCollider;
        private bool _bWasTriggered;
        public event Action OnTriggered = delegate { };

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_bWasTriggered) return;
        
            if (other.gameObject.CompareTag(triggerName))
            {
                OnTriggered.Invoke();
                _bWasTriggered = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, .75f);
        }
    }
}
