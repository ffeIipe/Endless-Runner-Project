using Enums;
using UnityEngine.SceneManagement;

namespace ScreenManagerFolder
{
    public class DeathScreen : BaseScreen
    {
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