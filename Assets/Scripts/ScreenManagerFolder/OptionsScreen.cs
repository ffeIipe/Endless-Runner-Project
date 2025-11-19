using Managers;

namespace ScreenManagerFolder
{
    public class OptionsScreen : BaseScreen
    {
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