using Entities;
using Scriptables;
using UnityEngine;

namespace Obstacles
{
    public class Log : MonoBehaviour
    {
        [SerializeField] private LogData logData;
        private Trigger _trigger;
        
        private void Awake()
        {
            _trigger = GetComponent<Trigger>();
            _trigger.OnTriggeredCollider += ApplyDamage;
        }

        private void ApplyDamage(Collider other, Vector3 dir)
        {
            var entity = other.GetComponent<Entity>();

            if (!entity) return;
            entity.TakeDamage(logData.damage);
            entity.GetHit(dir, 10f);
        }
    }
}