using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PortalCamera : MonoBehaviour
{
    public Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
}
