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
        if (listeningLayer == (listeningLayer | (1 << other.gameObject.layer)))
        {
            triggerEnterEvent?.Raise();
            triggerInteractEvent?.Raise();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (listeningLayer == (listeningLayer | (1 << other.gameObject.layer)))
        {
            triggerExitEvent?.Raise();
            triggerInteractEvent?.Raise();
        }
    }

    public void TestTemp()
    {
        Debug.Log("YAY");
    }
}
