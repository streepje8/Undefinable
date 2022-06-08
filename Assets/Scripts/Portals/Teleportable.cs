using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportable : MonoBehaviour
{
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
        //rb.velocity = dest.transform.rotation * (Quaternion.Inverse(sender.transform.rotation) * rb.velocity);
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
