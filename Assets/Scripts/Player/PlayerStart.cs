using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerStart : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;
        
        private void Start()
        {
            var tempPlayer = GameManager.Instance.player;

            if (!tempPlayer)
            {
                tempPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            }
            
            tempPlayer.GetCharacterController().enabled = false;
            tempPlayer.transform.position = transform.position;
            tempPlayer.transform.rotation = transform.rotation;
            tempPlayer.GetCharacterController().enabled = true;

            EventManager.GameEvents.OnLevelStarted.Invoke();
        }
    }
}