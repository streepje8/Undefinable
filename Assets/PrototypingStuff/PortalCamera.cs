using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
}
