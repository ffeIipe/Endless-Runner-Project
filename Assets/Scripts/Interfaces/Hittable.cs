
using UnityEngine;

namespace Interfaces
{
    public interface IHittable
    {
        public void TakeDamage(float damage);
        
        public void GetHit(Vector3 hitPoint, float force);
    }
}