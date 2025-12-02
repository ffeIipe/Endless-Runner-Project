using Enums;
using Managers;
using UnityEngine.SceneManagement;

namespace ScreenManagerFolder
{
    public class LevelSelectionScreen : BaseScreen
    {
        public void LoadSelectedScene(string level)
        {
            GameManager.Instance.LoadLevel(level);
            GameManager.Instance.ResumeGame(ScreenType.Gameplay);
        }

        public void Back()
        {
            GameManager.Instance.HandlePauseInput();
        }
    }
}