using Enums;
using Pool;
using Scriptables.Abilities;
using UnityEngine;

namespace Entities.Abilities
{
    public abstract class Ability : MonoBehaviour, IPoolable
    {
        [SerializeField] protected AbilityData abilityData;
        private TeamType _ownerTeam;
        
        public Entity Owner { get; set; }
        public abstract void Activate();
        public abstract void Deactivate();
        
        public virtual void SetOwner(Entity owner)
        {
            Owner = owner;
            _ownerTeam = owner.GetTeam();
        }
    }
}