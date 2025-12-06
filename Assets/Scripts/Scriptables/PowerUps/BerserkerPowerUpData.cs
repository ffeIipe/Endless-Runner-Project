using UnityEngine;

namespace Scriptables.PowerUps
{
    [CreateAssetMenu(fileName = "BerserkerPowerUpData", menuName = "PowerUps/BerserkerPowerUpData", order = 0)]
    public class BerserkerPowerUpData : PowerUpData
    {
        public float speedMultiplier = 1.5f;
    }
}