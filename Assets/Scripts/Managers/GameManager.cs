using System;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static bool isPaused;

        private void Awake()
        {
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
            if (!isPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            EventManager.Instance.gameEvents.Pause.Invoke(isPaused);

            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}