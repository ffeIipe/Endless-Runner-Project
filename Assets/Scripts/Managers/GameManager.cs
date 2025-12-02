using System;
using System.Collections;
using Enums;
using ScreenManagerFolder;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Camera mainMenuCamera;
        public Player.Player player;
        public static GameManager Instance { get; private set; }
        public static bool IsPaused { get; private set; }
        
        private string _currentLevelName;
        private float _startingLevelTime;
        
        private void Awake()
        {
            IsPaused = false;
            
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            EventManager.GameEvents.OnLevelStarted += StartLevelTimer;
        }

        private void OnDisable()
        {
            EventManager.GameEvents.OnLevelStarted -= StartLevelTimer;
        }
        
        private void Start()
        {
            Time.timeScale = 1;
            
            ScreenManager.Instance.PushScreen(ScreenType.MainMenu, true);
        }

        public void LoadLevel(string level, Action onComplete = null)
        {
            _currentLevelName = level;
            
            onComplete += () =>
            {
                player.gameObject.SetActive(true);
                mainMenuCamera.gameObject.SetActive(false);
            };
            
            StartCoroutine(LoadLevelRoutine(level, onComplete));
        }

        private IEnumerator LoadLevelRoutine(string level, Action onComplete = null)
        {
            if (SceneManager.GetSceneByName(_currentLevelName).name == level || 
                SceneManager.GetSceneByName(_currentLevelName).name == "PersistentGameplay") 
                yield break;
            
            var asyncLoad = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);

            while (asyncLoad != null && !asyncLoad.isDone)
            {
                yield return null;
            }

            var currentScene = SceneManager.GetSceneByName(level);
            SceneManager.SetActiveScene(currentScene);
            onComplete?.Invoke();
        }

        public void RestartCurrentLevel(Action onRestarted = null)
        {
            onRestarted += () =>
            {
                Time.timeScale = 1;
                EventManager.PlayerEvents.OnNewAttempt.Invoke();
            };
            
            StartCoroutine(RestartLevelRoutine(onRestarted));
        }

        IEnumerator RestartLevelRoutine(Action onRestarted)
        {
            if (IsPaused) IsPaused = false;
            if(Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;
            
            var unload = SceneManager.UnloadSceneAsync(_currentLevelName);
        
            while (unload != null && !unload.isDone) yield return null;

            var load = SceneManager.LoadSceneAsync(_currentLevelName, LoadSceneMode.Additive);
        
            while (load != null && !load.isDone) yield return null;

            EffectsManager.Instance.PlayFadeScreen(true);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentLevelName));

            onRestarted?.Invoke();
            EventManager.GameEvents.OnLevelRestarted?.Invoke();
        }
        
        public void HandlePauseInput()
        {
            if (!player)
            {
                ScreenManager.Instance.PopScreen();
                return;
            }

            if (!player.GetAttributesComponent().IsAlive()) return;

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

        public void ResumeGame(ScreenType screen = ScreenType.None)
        {
            IsPaused = false;
            if (screen == ScreenType.None)
                ScreenManager.Instance.PopScreen();

            else
                ScreenManager.Instance.PushScreen(screen, true);
            
            EventManager.GameEvents.Pause.Invoke(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void StartLevelTimer()
        {
            _startingLevelTime = Time.time;
        }

        public float GetLevelTime()
        {
             return Time.time - _startingLevelTime;
        }
    }
}