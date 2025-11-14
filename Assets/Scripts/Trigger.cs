using System;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class Trigger : MonoBehaviour
{
    public event Action<Collider> OnPlayerEnter = delegate { };

    protected virtual void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerEnter.Invoke(other);  
        }
    }
}
