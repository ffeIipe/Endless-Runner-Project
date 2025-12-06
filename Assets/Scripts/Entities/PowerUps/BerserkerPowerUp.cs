using Scriptables.PowerUps;

namespace Entities.PowerUps
{
    public class BerserkerPowerUp : PowerUp
    {
        private BerserkerPowerUpData BerserkerPowerUpData => (BerserkerPowerUpData) powerUpData;
        
        protected override void ApplyEffect(Entity user)
        {
            base.ApplyEffect(user);

            if (user.TryGetComponent(out Player.Player player))
            {
                player.GetModel().MaxSpeed *= BerserkerPowerUpData.speedMultiplier;
            }
        }

        protected override void RemoveEffect()
        {
            base.RemoveEffect();
            
            if (Owner.TryGetComponent(out Player.Player player))
            {
                player.GetModel().MaxSpeed /= BerserkerPowerUpData.speedMultiplier;
            }
        }
    }
}