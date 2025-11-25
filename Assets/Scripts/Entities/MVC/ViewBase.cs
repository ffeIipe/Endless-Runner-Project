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

        public virtual void OnShieldDamaged()
        {
            
        }

        public virtual void ApplyDamageEffect(Vector3 hitPoint, Vector3 hitNormal, float force)
        {
            //vfx dmg particle
        }

        public virtual void HeadShotEffect()
        {
            
        }
    }
}