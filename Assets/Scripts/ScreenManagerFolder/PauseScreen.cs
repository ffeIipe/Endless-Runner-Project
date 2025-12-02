using Enums;
using Managers;
using UnityEngine.SceneManagement;

namespace ScreenManagerFolder
{
    public class PauseScreen : BaseScreen
    {
        public void Resume()
        {
            GameManager.Instance.ResumeGame();
        }

        public void Options()
        {
            ScreenManager.Instance.PushScreen(ScreenType.Options, true);
        }

        public void Restart()
        {
            GameManager.Instance.RestartCurrentLevel(() => ScreenManager.Instance.PushScreen(ScreenType.Gameplay, true));
        }
        
        public void MainMenu()
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            ScreenManager.Instance.PushScreen(ScreenType.MainMenu, true);
        }
    }
}
