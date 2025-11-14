using Interfaces;
using UnityEngine;

public class DamageBox : Trigger
{
    private float _damage;
    
    public void SetDamage(float damage)
    {
        _damage = damage;
    }
    
    protected override void Awake()
    {
        base.Awake();

        OnPlayerEnter += Damage;
    }
    
    private void Damage(Collider obj)
    {
        var hittable = obj.gameObject.GetComponentInParent<IHittable>();

        if (_damage == 0f)
        {
            Debug.Log("Damage equals zero");
        }
        hittable?.TakeDamage(_damage);
    }
}
