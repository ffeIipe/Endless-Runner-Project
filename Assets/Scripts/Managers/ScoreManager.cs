using Structs;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        public Score Score;
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnEnable()
        {
            EventManager.PlayerEvents.OnNewAttempt += IncreaseAttempts;
            EventManager.PlayerEvents.OnPlayerDamaged += IncreaseDamageTaken;
            EventManager.PlayerEvents.OnEnemyKilled += IncreaseEnemiesKilled;
            EventManager.PlayerEvents.OnAxeThrown += IncreaseAxesThrown;
            EventManager.PlayerEvents.OnPowerUpPickedUp += IncreasePowerUpsPickedUp;
            EventManager.PlayerEvents.OnTrapOpened += IncreaseTrapsOpened;
        }

        public void ClearScore()
        {
            Score.Clear();
        }

        private void IncreaseAttempts()
        {
            Score.Attempts++;
        }

        private void IncreaseDamageTaken()
        {
            Score.DamageTaken++;
        }
        
        private void IncreaseAxesThrown()
        {
            Score.AxesThrown++;
        }
        
        private void IncreaseEnemiesKilled()
        {
            Score.EnemiesKilled++;
        }
        
        private void IncreasePowerUpsPickedUp()
        {
            Score.PowerUpsPickedUp++;
        }
        
        private void IncreaseTrapsOpened()
        {
            Score.TrapsOpened++;
        }
    }
}