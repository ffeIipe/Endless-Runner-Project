using Enums;
using Managers;
using UnityEngine;

namespace Obstacles
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private PoolableType enemyToSpawn;

        private void Awake()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var hit, 100f, LayerMask.GetMask("Ground")))
            {
                transform.position = hit.point;
                Debug.Log("Setting " + name + " at " + transform.position);
            }
        }

        public void Spawn()
        {
            FactoryManager.Instance.SpawnObject(
                enemyToSpawn,
                transform.position,
                transform.rotation
            );
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
    }
}