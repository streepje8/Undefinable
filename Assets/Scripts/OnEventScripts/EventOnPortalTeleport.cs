using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Openverse.Events;

public class EventOnPortalTeleport : MonoBehaviour, ITeleportListener
{
    public GameEvent eventToRaise;
    public void OnTeleport(Portal sender, Portal reciepient)
    {
        eventToRaise?.Raise();
    }

}
