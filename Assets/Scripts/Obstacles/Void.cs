using UnityEngine;

namespace Obstacles
{
    
    [RequireComponent(typeof(Trigger))]
    public class Void :  MonoBehaviour
    {
        private Trigger _trigger;

        private void Awake()
        {
            _trigger = GetComponent<Trigger>();
        }

        private void OnEnable()
        {
            _trigger.OnTriggeredCollider += OnVoidTriggered;
        }

        private void OnDisable()
        {
            _trigger.OnTriggeredCollider -= OnVoidTriggered;
        }
        
        private void OnVoidTriggered(Collider coll, Vector3 contact)
        {
            if (coll.gameObject.TryGetComponent(out Player.Player player))
            {
                player.TakeDamage(100f, null);
            }
            else Debug.Log("Not the player");
        }
    }
}