using System.Collections;
using Entities;
using Managers;
using Scriptables;
using UnityEngine;

namespace Obstacles
{
    public class Log : MonoBehaviour
    {
        [SerializeField] private LogData logData;
        [SerializeField] private bool isMovable;

        [SerializeField] private Trigger movementTrigger;
        [SerializeField] private Trigger damageTrigger;
        
        [SerializeField] private GameObject logModel;
        
        private void Awake()
        {
            if (isMovable)
            {
                movementTrigger.OnTriggered += StartMovingLog;
            }
            
            damageTrigger.OnTriggeredCollider += ApplyDamage;
        }

        private void StartMovingLog()
        {
            StartCoroutine(MoveLog());
        }

        private IEnumerator MoveLog()
        {
            var timer = 0f;
    
            while (timer < logData.duration)
            {
                if (!GameManager.IsPaused)
                {
                    timer += Time.deltaTime;
                    
                    transform.Translate(Vector3.forward * (logData.speed * Time.deltaTime));
        
                    if (logModel)
                    {
                        logModel.transform.Rotate(Vector3.up, logData.speed * -20 * Time.deltaTime, Space.Self);
                    }   
                }

                yield return null;
            }
    
            //Effect
            // Destroy(gameObject); 
        }

        private void ApplyDamage(Collider other, Vector3 dir)
        {
            var entity = other.GetComponent<Entity>();

            if (!entity) return;
            entity.TakeDamage(logData.damage);
            entity.GetHit(dir, dir, 10f);
        }
    }
}