using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-(Camera.main.transform.position - transform.position).normalized,Vector3.up), 10f * Time.deltaTime);
    }
}
