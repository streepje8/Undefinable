using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public BillboardSettings constraints;
    void Update()
    {
        Vector3 rotationthing = Quaternion.LookRotation(-(Camera.main.transform.position - transform.position).normalized, Vector3.up).eulerAngles;
        rotationthing.x *= constraints.lockX ? 0 : 1;
        rotationthing.y *= constraints.lockY ? 0 : 1;
        rotationthing.z *= constraints.lockZ ? 0 : 1;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotationthing), 10f * Time.deltaTime);
    }

    [System.Serializable]
    public struct BillboardSettings
    {
        public bool lockX;
        public bool lockY;
        public bool lockZ;
    }
}

