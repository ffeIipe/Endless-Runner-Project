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
            //DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (!IsPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void TogglePause()
        {
            if (!player.GetAttributesComponent().IsAlive()) return;
            
            IsPaused = !IsPaused;
            EventManager.GameEvents.Pause.Invoke(IsPaused);
    
            Cursor.lockState = IsPaused ? CursorLockMode.None : CursorLockMode.Locked;

            if (IsPaused)
            {
                ScreenManager.Instance.PushScreen(ScreenType.PauseMenu, false);
            }
            else
            {
                ScreenManager.Instance.PopScreen();
            }
        }
    }
}