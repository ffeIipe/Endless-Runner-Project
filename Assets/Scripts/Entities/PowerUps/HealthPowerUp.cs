using Scriptables;

namespace Entities.PowerUps
{
    public class HealthPowerUp : PowerUp
    {
        private HealthPowerUpData _healthPowerUpData;
        protected override void Awake()
        {
            base.Awake();
            
            _healthPowerUpData = (HealthPowerUpData)powerUpData;
        }

        protected override void ApplyEffect(Entity user)
        {
            base.ApplyEffect(user);
            
            user.GetAttributesComponent().IncreaseHealth(_healthPowerUpData.health);
        }
    }
}