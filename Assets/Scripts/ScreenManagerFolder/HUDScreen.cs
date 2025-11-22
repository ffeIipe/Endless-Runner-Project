using Managers;
using TMPro;
using UnityEngine;

namespace ScreenManagerFolder
{
    public class HUDScreen : BaseScreen
    {
        [SerializeReference] private TextMeshProUGUI healthText;
        [SerializeReference] private TextMeshProUGUI killsText;
        [SerializeReference] private TextMeshProUGUI timesDamagedText;
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
            
            var t = Mathf.Clamp01(velocity / 22f);

            var finalColor = t < 0.5f ? 
                Color.Lerp(Color.red, Color.yellow, t * 2f) : 
                Color.Lerp(Color.yellow, Color.green, (t - 0.5f) * 2f);

            velocityText.color = finalColor;
        }

        private void OnDestroy()
        {
            EventManager.UIEvents.OnVelocityChanged -= SetVelocityText;
            EventManager.UIEvents.OnHealthChanged -= SetHealthText;
        }
    }
}