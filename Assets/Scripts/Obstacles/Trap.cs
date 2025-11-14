using System;
using System.Collections;
using Scriptables;
using UnityEngine;

namespace Obstacles
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private TrapData trapData;
        [SerializeField] private Transform hinge;
        
        [SerializeField] private Trigger trigger;
        [SerializeField] private DamageBox damageBox;
        private bool _wasTriggered;
    
        private void Awake()
        {
            if (trigger != null)
            {
                trigger.OnPlayerEnter += RotateHinge;
            }
            else
            {
                Debug.Log("Trap's trigger not assigned");
            }

            if (damageBox != null)
            {
                damageBox.SetDamage(trapData.damage);
            }
            else
            {
                Debug.Log("Trap's damage box not assigned");
            }
        }

        private void RotateHinge(Collider obj)
        {
            if (_wasTriggered) return;
            _wasTriggered = true;

            StartCoroutine(StartHingeRotation());
        }

        private IEnumerator StartHingeRotation()
        {
            var elapsedTime = 0f;
        
            var startRotation = hinge.localRotation;
            var targetRotation = Quaternion.Euler(0, 0, trapData.rotationAngle);

            while (elapsedTime < trapData.rotationDuration)
            {
                hinge.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / trapData.rotationDuration);
            
                elapsedTime += Time.deltaTime;
            
                yield return null; 
            }
        
            hinge.localRotation = targetRotation;
        }

        private void OnDrawGizmos()
        {
            hinge.transform.localScale = new Vector3(1, trapData.damageBoxSize, 1);
        }
    }
}
