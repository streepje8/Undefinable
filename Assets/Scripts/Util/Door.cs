using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 rotationOffset = Vector3.zero;
    public int openAngle = 145;
    public bool interactable = true;
    private int rotation = 0;
    public void ToggleOpenClosed()
    {
        if (interactable)
            rotation = rotation < 10 ? openAngle : 0;
    }

    public void SetDoorState(bool state)
    {
        rotation = state ? 145 : 0;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotationOffset + Vector3.up * rotation), 3f * Time.deltaTime);
    }
}
