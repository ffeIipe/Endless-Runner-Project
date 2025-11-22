using System;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }
        public static readonly PlayerEvents PlayerEvents = new();
        public static readonly GameEvents GameEvents = new();
        public static readonly UIEvents UIEvents = new();
        
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
    }

    public class PlayerEvents
    {
        public Action OnPlayerDead;
    }
    
    public class GameEvents
    {
        public UnityAction<bool> Pause;
    }
        
    public class UIEvents
    {
        public Action<float> OnSensitivityChanged =  delegate { };
        public Action<float> OnSoundVolumeChanged = delegate { };
        public Action<float> OnVelocityChanged = delegate { };
        public Action<float> OnHealthChanged = delegate { };
    }
}