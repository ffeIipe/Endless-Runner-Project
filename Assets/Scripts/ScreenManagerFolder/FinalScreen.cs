using Enums;
using UnityEngine.SceneManagement;

namespace ScreenManagerFolder
{
    public class FinalScreen : BaseScreen
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