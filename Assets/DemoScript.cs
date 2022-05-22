using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public Vector3 rotationSpeed = Vector3.zero;
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
