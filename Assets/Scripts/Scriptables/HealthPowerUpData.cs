using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "PowerUpData", menuName = "PowerUpData", order = 0)]
    public class HealthPowerUpData : PowerUpData
    {
        public float health;
    }
}