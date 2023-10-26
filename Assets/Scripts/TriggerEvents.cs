using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    public UnityEvent triggerEnterEvent;
    public UnityEvent triggerStayEvent;
    public UnityEvent triggerExitEvent;
    public string eventTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(eventTrigger))
            triggerEnterEvent?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        triggerStayEvent?.Invoke(); 
    }

    private void OnTriggerExit(Collider other)
    {
        triggerExitEvent?.Invoke();
    }
}
