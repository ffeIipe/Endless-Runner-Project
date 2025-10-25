using Scriptables;
using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public EntityData entityData;

        public float GetHealth()
        {
            return entityData.health;
        }
    }
}