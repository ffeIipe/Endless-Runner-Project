using Scriptables.PowerUps;

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

        public override void PickUp(Entity user)
        {
            base.PickUp(user);
            
            user.GetAttributesComponent().IncreaseHealth(_healthPowerUpData.health);
        }
    }
}