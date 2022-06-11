using Openverse.Events;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public LayerMask listeningLayer;
    public GameEvent triggerEnterEvent;
    public GameEvent triggerExitEvent;
    public GameEvent triggerInteractEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (listeningLayer == (listeningLayer | (1 << other.gameObject.layer))) //Check if the object is on the listening layer
        {
            triggerEnterEvent?.Raise();
            triggerInteractEvent?.Raise();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (listeningLayer == (listeningLayer | (1 << other.gameObject.layer))) //Check if the object is on the listening layer
        {
            triggerExitEvent?.Raise();
            triggerInteractEvent?.Raise();
        }
    }
}
