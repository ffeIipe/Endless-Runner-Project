using Enums;
using Managers;

namespace ScreenManagerFolder
{
    public class LevelSelectionScreen : BaseScreen
    {
        public void LoadSelectedScene(int sceneIndex)
        {
            GameManager.Instance.LoadLevel(sceneIndex, OnLoadComplete, OnLoadFailed); return;

            void OnLoadComplete()
            {
                GameManager.Instance.ResumeGame(ScreenType.Gameplay);
            }
            
            void OnLoadFailed()
            {
                ScreenManager.Instance.PushScreen(ScreenType.MainMenu, true);
            }
        }
        
        public void Back()
        {
            GameManager.Instance.HandlePauseInput();
        }
    }
}