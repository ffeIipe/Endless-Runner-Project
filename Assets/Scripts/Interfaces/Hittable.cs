
using Entities;
using UnityEngine;

namespace Interfaces
{
    public interface IHittable
    {
        public void TakeDamage(float damage, Entity damageCauser);
        
        public void GetHit(Vector3 direction, Vector3 hitPoint, Vector3 hitNormal, float force);
    }
}