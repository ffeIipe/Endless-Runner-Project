using Structs;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public ScoreManager Instance {get; private set;}

        [SerializeField] private Score _score;
        
        private void Awake()
        {
            if (Instance ==null)
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
            EventManager.PlayerEvents.OnPlayerDead += IncreaseAttempts;
            EventManager.PlayerEvents.OnPlayerDamaged += IncreaseTimesDamaged;
            EventManager.PlayerEvents.OnEnemyKilled += IncreaseEnemiesKilled;
            EventManager.PlayerEvents.OnPowerUpPickedUp += IncreasePowerUpsPickedUp;
            EventManager.PlayerEvents.OnTrapOpened += IncreaseTrapsOpened;
        }

        public void ClearScore()
        {
            _score.Clear();
        }

        private void IncreaseAttempts()
        {
            _score.Attempts++;
        }

        private void IncreaseTimesDamaged()
        {
            _score.TimesDamaged++;
        }
        
        private void IncreaseEnemiesKilled()
        {
            _score.EnemiesKilled++;
        }
        
        private void IncreasePowerUpsPickedUp()
        {
            _score.PowerUpsPickedUp++;
        }
        
        private void IncreaseTrapsOpened()
        {
            _score.TrapsOpened++;
        }
    }
}