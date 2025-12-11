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
        [SerializeReference] private TextMeshProUGUI fpsText;
        [SerializeReference] private Slider healthBar;

        private float _time;
        private int _frameCount;
        
        
        private void OnEnable()
        {
            EventManager.UIEvents.OnVelocityChanged += SetVelocityText;
            EventManager.UIEvents.OnHealthPercentageChanged += SetHealthBar;
        }
        
        private void Update()
        {
            SetTimeText(GameManager.Instance.GetLevelTime());
            
            _time += Time.deltaTime;
            _frameCount++;

            if (!(_time >= 0.5f)) return;
            
            var frameRate = Mathf.RoundToInt(_frameCount / _time);

            var displayColor = frameRate switch
            {
                < 30 => Color.red,
                < 60 => Color.yellow,
                _ => Color.green
            };

            if (fpsText)
            {
                fpsText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(displayColor)}>{frameRate} FPS</color>";
                fpsText.SetText(frameRate.ToString());
            }

            _time -= 0.5f;
            _frameCount = 0;
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