using System;

namespace Managers
{
    public abstract class EventManager
    {
        public static readonly PlayerEvents PlayerEvents = new();
        public static readonly GameEvents GameEvents = new();
        public static readonly UIEvents UIEvents = new();
    }

    public class PlayerEvents
    {
        public Action OnPlayerDead = delegate { };
        public Action OnPlayerDamaged = delegate { };
        public Action OnEnemyKilled = delegate { };
        public Action OnPowerUpPickedUp = delegate { };
        public Action OnTrapOpened = delegate { };
        public Action OnAxeThrown = delegate { };
        public Action OnNewAttempt = delegate { };
    }
    
    public class GameEvents
    {
        public Action<bool> Pause = delegate { };
        public Action OnLevelFinished = delegate { };
        public Action OnLevelRestarted = delegate { };
        public Action OnLevelStarted = delegate { };
        public Action OnLevelChanged = delegate { };
    }
        
    public class UIEvents
    {
        public Action<float> OnSensitivityChanged =  delegate { };
        public Action<float> OnSoundVolumeChanged = delegate { };
        public Action<float> OnVelocityChanged = delegate { };
        public Action<float> OnHealthPercentageChanged = delegate { };
    }
}