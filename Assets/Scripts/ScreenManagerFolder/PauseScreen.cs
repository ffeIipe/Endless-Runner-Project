using Enums;
using Managers;

namespace ScreenManagerFolder
{
    public class PauseScreen : BaseScreen
    {
        public override void Hide()
        {
            base.Hide();

            if (GameManager.isPaused)
            {
                GameManager.Instance.TogglePause();
            }
        }

        public void Options()
        {
            ScreenManager.Instance.PushScreen(ScreenType.Options, true);
        }
        
        public void MainMenu()
        {
            ScreenManager.Instance.PushScreen(ScreenType.MainMenu, true);
        }
    }
}
