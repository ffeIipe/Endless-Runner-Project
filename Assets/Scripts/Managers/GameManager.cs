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

        public bool isLevelFinished;
        
        private const int PersistentLevelBuildIndex = 0;
        private int _currentLevelBuildIndex;
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
            
            _currentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
        }

        public void LoadLevel(int levelIndex, Action onComplete = null, Action onFailed = null)
        {
            StartCoroutine(LoadLevelRoutine(levelIndex, onComplete, onFailed));
        }

        private IEnumerator LoadLevelRoutine(int levelIndex, Action onComplete = null, Action onFailed = null)
        {
            if (levelIndex == _currentLevelBuildIndex || levelIndex == PersistentLevelBuildIndex)
            {
                onFailed?.Invoke();
                yield break;
            }
            
            _currentLevelBuildIndex = levelIndex;
            
            yield return SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);
            
            var currentScene = SceneManager.GetSceneByBuildIndex(levelIndex);
            SceneManager.SetActiveScene(currentScene);
            
            player.gameObject.SetActive(true);
            mainMenuCamera.gameObject.SetActive(false);
            onComplete?.Invoke();
            
            isLevelFinished = false;
            EventManager.GameEvents.OnLevelUpdated.Invoke();
        }

        public void RestartCurrentLevel(Action onRestarted = null)
        {
            StartCoroutine(RestartLevelRoutine(onRestarted));
        }

        private IEnumerator RestartLevelRoutine(Action onRestarted)
        {
            if (IsPaused) IsPaused = false;
            
            if(Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;
            
            yield return SceneManager.UnloadSceneAsync(_currentLevelBuildIndex);

            yield return SceneManager.LoadSceneAsync(_currentLevelBuildIndex, LoadSceneMode.Additive);

            EffectsManager.Instance.PlayFadeScreen(true);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentLevelBuildIndex));

            Time.timeScale = 1;
            EventManager.PlayerEvents.OnNewAttempt.Invoke();
            ScreenManager.Instance.PushScreen(ScreenType.Gameplay, true);
            onRestarted?.Invoke();
            
            EventManager.GameEvents.OnLevelRestarted?.Invoke();
            EventManager.GameEvents.OnLevelUpdated.Invoke();
            isLevelFinished = false;
        }

        public void LoadNextLevel(Action onFinishLoading = null)
        {
            onFinishLoading += () =>
            {
                Time.timeScale = 1;
                EffectsManager.Instance.ResetEffects();
            };
            
            StartCoroutine(LoadNextLevelRoutine(onFinishLoading));
        }
        
        private IEnumerator LoadNextLevelRoutine(Action onFinishLoading)
        {
            if (IsPaused) IsPaused = false;
            if (Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;

            yield return SceneManager.UnloadSceneAsync(_currentLevelBuildIndex);

            var nextSceneIndex = _currentLevelBuildIndex + 1;

            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                ScreenManager.Instance.PushScreen(ScreenType.MainMenu, true);
                _currentLevelBuildIndex = PersistentLevelBuildIndex; 
        
                yield break; 
            }
    
            yield return SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);

            EffectsManager.Instance.PlayFadeScreen(true);
    
            var nextSceneObj = SceneManager.GetSceneByBuildIndex(nextSceneIndex);
            if (nextSceneObj.IsValid())
            {
                SceneManager.SetActiveScene(nextSceneObj);
                _currentLevelBuildIndex = nextSceneIndex;
            }

            onFinishLoading?.Invoke();
            EventManager.GameEvents.OnLevelChanged?.Invoke();
            EventManager.GameEvents.OnLevelUpdated.Invoke();
            
            isLevelFinished = false;
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