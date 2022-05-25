using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITeleportListener
{
    void OnTeleport(Vector3 positionChange, Quaternion rotationChange);   
}
