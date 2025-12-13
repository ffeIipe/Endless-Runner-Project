using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenManagerFolder
{
    public class OptionsScreen : BaseScreen
    {
        [SerializeField] private Slider soundSlider; 
        [SerializeField] private Slider sensitivitySlider; 
        
        private void Start()
        {
            sensitivitySlider.value = .5f;
            soundSlider.value = .2f;
        }

        public void Back()
        {
            ScreenManager.Instance.PopScreen();
        }
        
        public void OnSoundChanged(float value)
        {
            EventManager.UIEvents.OnSoundVolumeChanged(value);
        }
        
        public void OnSensitivityChanged(float value)
        {
            EventManager.UIEvents.OnSensitivityChanged(value);
        }
    }
}