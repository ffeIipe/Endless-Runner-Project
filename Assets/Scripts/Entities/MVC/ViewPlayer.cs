using Managers;

namespace Entities.MVC
{
    public class ViewPlayer : ViewBase
    {
        private readonly Model _model;
        
        public ViewPlayer(Entity owner, Model model) : base(owner)
        {
            _model = model;
        }

        public void GetVelocity(float currentVelocity)
        {
            if (currentVelocity == 0f) return;
            
            EventManager.UIEvents.OnVelocityChanged.Invoke(currentVelocity);
            EffectsManager.Instance.UpdateFOV(currentVelocity);
        }
    }
}