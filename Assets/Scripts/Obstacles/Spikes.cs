using Entities;
using UnityEngine;

namespace Obstacles
{
    public class Spikes : MonoBehaviour
    {
        private float _damage;
        
        public void SetDamage(float damage) =>  _damage = damage; 
        private void OnTriggerEnter(Collider other)
        {
            var entity = other.GetComponent<Entity>();
            if (entity)
            {
                entity.TakeDamage(_damage);
                var dir = other.transform.position - transform.position;
                entity.GetHit(dir, dir, 10f);
            }
        }
    }
}
