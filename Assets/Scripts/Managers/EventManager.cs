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
        public Action OnPlayerDead;
        public Action OnPlayerDamaged;
        public Action OnEnemyKilled;
        public Action OnPowerUpPickedUp;
        public Action OnTrapOpened;
        public Action AxesThrown;
    }
    
    public class GameEvents
    {
        public Action<bool> Pause = delegate { };
        public Action<bool> IsLevelFinished = delegate { };
    }
        
    public class UIEvents
    {
        public Action<float> OnSensitivityChanged =  delegate { };
        public Action<float> OnSoundVolumeChanged = delegate { };
        public Action<float> OnVelocityChanged = delegate { };
        public Action<float> OnHealthChanged = delegate { };
    }
}