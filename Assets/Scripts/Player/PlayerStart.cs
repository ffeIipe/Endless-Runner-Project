using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerStart : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;
        
        private void Awake()
        {
            var tempPlayer = GameManager.Instance.player;

            if (!tempPlayer)
            {
                tempPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            }
            
            SceneManager.MoveGameObjectToScene(tempPlayer.gameObject, SceneManager.GetSceneByName("PersistentGameplay"));
            
            tempPlayer.GetCharacterController().enabled = false;
            tempPlayer.transform.position = transform.position;
            tempPlayer.transform.rotation = transform.rotation;
            tempPlayer.GetCharacterController().enabled = true;

            EventManager.GameEvents.OnLevelStarted.Invoke();
        }
    }
}