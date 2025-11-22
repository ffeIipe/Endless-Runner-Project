using Managers;
using TMPro;
using UnityEngine;

namespace ScreenManagerFolder
{
    public class HUDScreen : BaseScreen
    {
        [SerializeReference] private TextMeshProUGUI healthText;
        [SerializeReference] private TextMeshProUGUI velocityText;

        private void Start()
        {
            EventManager.UIEvents.OnVelocityChanged += SetVelocityText;
            EventManager.UIEvents.OnHealthChanged += SetHealthText;
        }

        private void SetHealthText(float health)
        {
            healthText.SetText(health.ToString());
        }
        
        private void SetVelocityText(float velocity)
        {
            velocityText.SetText(velocity.ToString());
        }

        private void OnDestroy()
        {
            EventManager.UIEvents.OnVelocityChanged -= SetVelocityText;
            EventManager.UIEvents.OnHealthChanged -= SetHealthText;
        }
    }
}