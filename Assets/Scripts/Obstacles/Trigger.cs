using System;
using UnityEngine;

namespace Obstacles
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class Trigger : MonoBehaviour
    {
        [SerializeField] private string triggerName;
        [SerializeField] private bool triggerOnce;
        
        private BoxCollider _boxCollider;
        private bool _bWasTriggered;
        public event Action OnTriggered = delegate { };
        public event Action<Collider, Vector3> OnTriggeredCollider = delegate { };

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_bWasTriggered && triggerOnce) return;
        
            if (other.gameObject.CompareTag(triggerName))
            {
                OnTriggered?.Invoke();
                
                var dir =  (other.transform.position - transform.position).normalized;
                OnTriggeredCollider?.Invoke(other, dir);
                
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
