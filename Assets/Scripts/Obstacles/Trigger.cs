using System;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public sealed class Trigger : MonoBehaviour
{
    [SerializeField] private string triggerName; 
    
    private bool _bWasTriggered;
    public event Action OnTriggered = delegate { };

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_bWasTriggered) return;
            _bWasTriggered = true;
        
        if (other.gameObject.CompareTag(triggerName))
        {
            OnTriggered.Invoke();  
        }
    }
}
