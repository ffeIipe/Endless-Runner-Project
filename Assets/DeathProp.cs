using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class DeathProp : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private SphereCollider _collider;
    private Transform _savedTransform;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        
        _collider = GetComponent<SphereCollider>();
        _collider.enabled = false;
        
        _savedTransform = transform;
    }

    public void ActivateDeathProp(Vector3 velocity)
    {
        transform.parent = null;
        _collider.enabled = true;
        
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(velocity.normalized * 150f, ForceMode.Impulse);
    }

    public void ResetDeathProp()
    {
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        transform.parent = _savedTransform;
        transform.SetLocalPositionAndRotation(_savedTransform.localPosition, _savedTransform.localRotation);
    }
}
