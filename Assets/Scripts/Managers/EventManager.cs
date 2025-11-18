using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }
        public readonly GameEvents gameEvents = new();

        public class GameEvents
        {
            public UnityAction<bool> Pause;
        }
        
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
}