using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportable : MonoBehaviour
{
    private bool teleported;
    private Quaternion lastRotationChange;
    private Vector3 lastPositionChange;

    private Rigidbody rb;
    private void OnEnable()
    {
        PortalManager.Instance.teleportables.Add(this);
        rb = GetComponent<Rigidbody>();
    }

    public void OnTeleport(Portal sender, Portal dest, Vector3 positionChange, Quaternion rotationChange)
    {
        teleported = true;
        lastPositionChange = positionChange;
        lastRotationChange = rotationChange;
        rb.velocity = dest.transform.rotation * (Quaternion.Inverse(sender.transform.rotation) * rb.velocity);
    }

    private void FixedUpdate()
    {
    }

    private void OnDisable()
    {
        if(PortalManager.Instance.teleportables.Contains(this))
        {
            PortalManager.Instance.teleportables.Remove(this);
        }
    }

    private void OnDrawGizmos()
    {
        if(rb != null)
        {
            Debug.DrawLine(transform.position,transform.position + rb.velocity * 100);
        }
    }
}
