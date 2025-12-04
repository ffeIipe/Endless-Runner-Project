using System;
using Enums;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScreenManagerFolder
{
    public class FinalScreen : BaseScreen
    {
        [SerializeField] private TextMeshProUGUI enemiesKilledText;
        [SerializeField] private TextMeshProUGUI axesThrownText;
        [SerializeField] private TextMeshProUGUI totalDamageTakenText;
        [SerializeField] private TextMeshProUGUI totalAttempts;
        [SerializeField] private TextMeshProUGUI totalTime;

        public override void Show()
        {
            base.Show();

            enemiesKilledText.SetText("Enemies killed: " + ScoreManager.Instance.Score.EnemiesKilled);
            axesThrownText.SetText("Axes thrown: " + ScoreManager.Instance.Score.AxesThrown);
            totalDamageTakenText.SetText("Damage taken: " + ScoreManager.Instance.Score.DamageTaken);
            totalAttempts.SetText("Total attempts: " + ScoreManager.Instance.Score.Attempts);
            
            var timeSpan = TimeSpan.FromSeconds(GameManager.Instance.GetLevelTime());
            var formattedTime = timeSpan.ToString(@"mm\:ss\.ff");
            totalTime.SetText("Total time: " + formattedTime);
        }

        public void NextLevel()
        {
            GameManager.Instance.LoadNextLevel(OnLevelLoaded);
        }

        private void OnLevelLoaded()
        {
            ScreenManager.Instance.PushScreen(ScreenType.Gameplay, true);
        }

        public void MainMenu()
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            ScreenManager.Instance.PushScreen(ScreenType.MainMenu, true);
        }
    }
}