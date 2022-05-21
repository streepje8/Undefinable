using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Teleportable : MonoBehaviour
{
    internal Vector3 direction = Vector3.zero;
    private Vector3 lastpos = Vector3.zero;
    private void OnEnable()
    {
        PortalManager.Instance.teleportables.Add(this);
        direction = Vector3.zero;
        lastpos = transform.position;
    }

    private void FixedUpdate()
    {
        direction = transform.position - lastpos;
        lastpos = transform.position;
    }

    private void OnDisable()
    {
        if(PortalManager.Instance.teleportables.Contains(this))
        {
            PortalManager.Instance.teleportables.Remove(this);
        }
    }
}
