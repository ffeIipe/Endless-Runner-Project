using UnityEngine;

namespace Scriptables.PowerUps
{
    [CreateAssetMenu(fileName = "ShieldPowerUpData", menuName = "PowerUps/ShieldPowerUpData", order = 0)]
    public class ShieldPowerUpData : PowerUpData
    {
        public float shield;
    }
}