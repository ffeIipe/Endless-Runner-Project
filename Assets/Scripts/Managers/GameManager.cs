using Enums;
using ScreenManagerFolder;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static bool IsPaused { get; private set; }
        public Player.Player player;

        private void Awake()
        {
            IsPaused = false;
            
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (!IsPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void HandlePauseInput()
        {
            if (!player || !player.GetAttributesComponent().IsAlive()) return;

            if (!IsPaused)
            {
                PauseGame();
            }
            else
            {
                var currentScreen = ScreenManager.Instance.GetCurrentScreenType();

                if (currentScreen == ScreenType.PauseMenu)
                {
                    ResumeGame();
                }
                else
                {
                    ScreenManager.Instance.PopScreen();
                }
            }
        }

        private void PauseGame()
        {
            if (!player.GetAttributesComponent().IsAlive()) return;

            if (IsPaused)
            {
                ScreenManager.Instance.PopScreen();
            }
            
            IsPaused = true;
            ScreenManager.Instance.PushScreen(ScreenType.PauseMenu, false);
            EventManager.GameEvents.Pause.Invoke(true);
            Cursor.lockState = CursorLockMode.None;
        }

        public void ResumeGame()
        {
            IsPaused = false;
            ScreenManager.Instance.PopScreen();
            EventManager.GameEvents.Pause.Invoke(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}