using UnityEngine;

namespace Scriptables.PowerUps
{
    [CreateAssetMenu(fileName = "HealthPowerUpData", menuName = "PowerUps/HealthPowerUpData", order = 0)]
    public class HealthPowerUpData : PowerUpData
    {
        public float health;
    }
}