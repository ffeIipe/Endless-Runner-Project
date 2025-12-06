using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenManagerFolder
{
    public class HUDScreen : BaseScreen
    {
        [SerializeReference] private TextMeshProUGUI velocityText;
        [SerializeReference] private TextMeshProUGUI timeText;
        [SerializeReference] private Slider healthBar;

        private void OnEnable()
        {
            EventManager.UIEvents.OnVelocityChanged += SetVelocityText;
            EventManager.UIEvents.OnHealthPercentageChanged += SetHealthBar;
        }
        
        private void Update()
        {
            SetTimeText(GameManager.Instance.GetLevelTime());
        }

        private void SetTimeText(float time)
        {
            var timeSpan = TimeSpan.FromSeconds(time);
            var formattedTime = timeSpan.ToString(@"mm\:ss\.ff");
            timeText.SetText(formattedTime);
        }

        private void SetHealthBar(float health)
        {
            healthBar.SetValueWithoutNotify(health);
        }
        
        private void SetVelocityText(float velocity)
        {
            velocityText.SetText(velocity.ToString("0")); 
            
            var t = Mathf.Clamp01(velocity / 22f);

            var finalColor = t < 0.5f ? 
                Color.Lerp(Color.red, Color.yellow, t * 2f) : 
                Color.Lerp(Color.yellow, Color.green, (t - 0.5f) * 2f);

            velocityText.color = finalColor;
        }

        private void OnDisable()
        {
            EventManager.UIEvents.OnVelocityChanged -= SetVelocityText;
            EventManager.UIEvents.OnHealthPercentageChanged -= SetHealthBar;
        }
    }
}