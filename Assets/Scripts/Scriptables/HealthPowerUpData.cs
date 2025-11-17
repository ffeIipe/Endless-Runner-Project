using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "HealthPowerUpData", menuName = "HealthPowerUpData", order = 0)]
    public class HealthPowerUpData : PowerUpData
    {
        public float health;
    }
}