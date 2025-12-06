using Enums;
using Managers;
using UnityEngine.SceneManagement;

namespace ScreenManagerFolder
{
    public class DeathScreen : BaseScreen
    {
        public void Restart()
        {
            GameManager.Instance.RestartCurrentLevel();
        }
        
        public void MainMenu()
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            ScreenManager.Instance.PushScreen(ScreenType.MainMenu, true);
        }
    }
}