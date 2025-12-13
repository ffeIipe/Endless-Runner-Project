using Enums;
using Managers;
using Managers.SoundManagerFolder;
using Scriptables.PowerUps;

namespace Entities.PowerUps
{
    public class ShieldPowerUp : PowerUp
    {
        private ShieldPowerUpData ShieldPowerUpData => (ShieldPowerUpData)powerUpData;

        public override void PickUp(Entity user)
        {
            base.PickUp(user);

            user.GetAttributesComponent().IncreaseShield(ShieldPowerUpData.shield);
            EffectsManager.Instance.ShieldEffect(true);
            
            SoundManager.Instance.PlaySound(SoundType.ShieldPowerUp, Owner.audioSource);
        }
    }
}