using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAnimator : MonoBehaviour
{
    public Vector3 speed;
    private Renderer rend;
    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
    }
    void Update()
    {
        if (rend == null || rend.isVisible)
            transform.Rotate(speed * 10f * Time.deltaTime);    
    }
}
