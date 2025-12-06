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
            sensitivitySlider.value = Mathf.Lerp(10f, 1000f, 500f/1000f);
        }

        public void Back()
        {
            ScreenManager.Instance.PopScreen();
        }
        
        public void OnSensitivityChanged(float value)
        {
            
            EventManager.UIEvents.OnSensitivityChanged(value);
        }
    }
}