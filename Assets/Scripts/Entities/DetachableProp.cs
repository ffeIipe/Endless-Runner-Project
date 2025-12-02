using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class DetachableProp : MonoBehaviour
    {
        private Entity _owner;
        
        public string propName;
        private Rigidbody _rigidbody;
        private SphereCollider _collider;
        private Vector3 _lastPosition;
        private Quaternion _lastRotation;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
        
            _collider = GetComponent<SphereCollider>();
            _collider.enabled = false;
            
            _owner = GetComponentInParent<Entity>();
            _lastPosition = transform.localPosition;
            _lastRotation = transform.localRotation;
        }

        public void ActivatePhysicsProp(Vector3 velocity)
        {
            transform.parent = null;
            _collider.enabled = true;
        
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(velocity.normalized * 150f, ForceMode.Impulse);
        }

        public void ResetPhysicsProp()
        {
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            transform.SetParent(_owner.transform);
            transform.SetLocalPositionAndRotation(_lastPosition, _lastRotation);
        }
    }
}
