using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinePositionalAnimator : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 amount = Vector3.zero;
    public Vector3 offset = Vector3.zero;

    private Vector3 startPosition = Vector3.zero;

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position = startPosition + (amount * Mathf.Sin(Time.time * (speed * Mathf.PI))) + offset;
    }
}
