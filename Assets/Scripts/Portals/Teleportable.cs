using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script must be attaches to any object that wants to teleport via portals, it registers the object at the portal manager for tracking
/// </summary>
public class Teleportable : MonoBehaviour
{
    [HideInInspector]public bool teleported;
    [HideInInspector]public Quaternion lastRotationChange;
    [HideInInspector]public Vector3 lastPositionChange;

    private Rigidbody rb;
    private void OnEnable()
    {
        PortalManager.Instance.teleportables.Add(this);
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// This function fixes the velocity of rigid bodies on teleport and stores all the data for when scripts want to check for teleports without becoming a listener
    /// </summary>
    /// <param name="sender">The sending portal</param>
    /// <param name="dest">The recieving portal</param>
    /// <param name="positionChange">The positional change</param>
    /// <param name="rotationChange">The rotational change</param>
    public void OnTeleport(Portal sender, Portal dest, Vector3 positionChange, Quaternion rotationChange)
    {
        lastPositionChange = positionChange;
        lastRotationChange = rotationChange;

        //Solution from https://answers.unity.com/questions/60709/portal-physics-effect.html
        if (rb != null)
        {
            var velocity = rb.velocity;
            velocity = Vector3.Reflect(velocity, sender.transform.forward);
            velocity = sender.transform.InverseTransformDirection(velocity);
            velocity = dest.transform.TransformDirection(velocity);
            rb.velocity = velocity;
        }
    }

    private void LateUpdate()
    {
        teleported = false; //Reset this at the end of the frame so other scripts can check if the entity has teleported in the update using if(teleported)
    }

    private void OnDisable()
    {
        if(PortalManager.Instance.teleportables.Contains(this))
        {
            PortalManager.Instance.teleportables.Remove(this);
        }
    }
}
