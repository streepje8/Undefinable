using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// You can extend this interface to listen for object teleports
/// </summary>
public interface ITeleportListener
{
    /// <summary>
    /// This function is called each time an object the script is attached to is teleported
    /// </summary>
    /// <param name="positionChange">The change in position that the object had</param>
    /// <param name="rotationChange">The change in rotation that the object had</param>
    void OnTeleport(Portal sender, Portal reciepient);   
}
