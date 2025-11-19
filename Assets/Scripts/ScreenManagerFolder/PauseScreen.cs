using Enums;
using Managers;
using UnityEngine.SceneManagement;

namespace ScreenManagerFolder
{
    public class PauseScreen : BaseScreen
    {
        public void Resume()
        {
            ScreenManager.Instance.PopScreen();
            
            if (GameManager.IsPaused)
            {
                GameManager.Instance.TogglePause();
            }
        }

        public void Options()
        {
            ScreenManager.Instance.PushScreen(ScreenType.Options, true);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public void MainMenu()
        {
            ScreenManager.Instance.PushScreen(ScreenType.MainMenu, true);
        }
    }
}
