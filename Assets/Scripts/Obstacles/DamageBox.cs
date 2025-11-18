using Interfaces;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    private float _damage;
    
    public void SetDamage(float damage)
    {
        _damage = damage;
    }
    
    protected void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        var hittable = other.gameObject.GetComponentInParent<IHittable>();

        if (_damage == 0f)
        {
            Debug.Log("Damage equals zero");
        }
        hittable?.TakeDamage(_damage);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var hittable = other.gameObject.GetComponentInParent<IHittable>();

        if (_damage == 0f)
        {
            Debug.Log("Damage equals zero");
        }
        hittable?.TakeDamage(_damage);
    }
}
