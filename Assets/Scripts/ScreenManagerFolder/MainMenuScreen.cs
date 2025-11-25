using Enums;
using UnityEngine;

namespace ScreenManagerFolder
{
    public class MainMenuScreen : BaseScreen
    {
        public void Play()
        {
            ScreenManager.Instance.PushScreen(ScreenType.LevelSelectionScreen, true);
        }

        public void Options()
        {
            ScreenManager.Instance.PushScreen(ScreenType.Options, false);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}