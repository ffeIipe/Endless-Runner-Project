using Enums;
using Managers;
using UnityEngine;

namespace Entities.MVC
{
    public class ViewBase
    {
        protected readonly Entity Owner;
        private RigidbodyConstraints _savedRigidbodyConstraints;

        public ViewBase(Entity owner)
        {
            Owner = owner;
        }

        public virtual void OnEntityDead()
        {
            Owner.GetRigidbody().isKinematic = false;
            _savedRigidbodyConstraints = RigidbodyConstraints.None;
            Owner.GetRigidbody().constraints = _savedRigidbodyConstraints;
        }

        public virtual void ApplyDamageEffect(Vector3 direction, Vector3 hitPoint, Vector3 hitNormal, float force)
        {
            var bloodFx = FactoryManager.Instance.SpawnObject(
                PoolableType.BloodEffect,
                hitPoint,
                Quaternion.Euler(hitNormal)
            );
            
            bloodFx.transform.position = hitPoint;
            bloodFx.transform.SetParent(Owner.transform);
        }

        public virtual void HeadShotEffect() { }

        public virtual void RestartEntityView() { }
    }
}