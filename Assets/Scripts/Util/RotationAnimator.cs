using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAnimator : MonoBehaviour
{
    public Vector3 speed;
    
    void Update()
    {
        transform.Rotate(speed * 10f * Time.deltaTime);    
    }
}
