using Managers;
using UnityEngine;

namespace Entities.MVC
{
    public class ViewPlayer : ViewBase
    {
        private readonly Model _model;
        private readonly ParticleSystem _snow;
        
        public ViewPlayer(Entity owner, Model model) : base(owner)
        {
            _model = model;
            _snow = owner.GetComponentInChildren<ParticleSystem>();
        }

        public void GetVelocity(float currentVelocity)
        {
            EventManager.UIEvents.OnVelocityChanged.Invoke(currentVelocity);
            EffectsManager.Instance.UpdateVelocityEffect(currentVelocity);
        }

        public void PauseEffects(bool pause)
        {
            if (pause)
                _snow.Pause();
            
            else
                _snow.Play();
        }

        public override void RestartEntityView()
        {
            EventManager.UIEvents.OnHealthPercentageChanged?.Invoke(Owner.GetAttributesComponent().GetHealthPercentage());
        }
    }
}