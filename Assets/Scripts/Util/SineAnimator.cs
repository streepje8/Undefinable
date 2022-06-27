using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineAnimator : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 amount = Vector3.zero;
    public Vector3 offset = Vector3.zero;
    private Renderer rend;
    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
    }
    void Update()
    {
        if(rend == null || rend.isVisible)
            transform.localRotation = Quaternion.Euler((amount * Mathf.Sin(Time.time * (speed * Mathf.PI))) + offset);        
    }
}