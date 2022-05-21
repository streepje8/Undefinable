using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : Singleton<PortalManager>
{
    public List<Teleportable> teleportables = new List<Teleportable>();
    private void Awake()
    {
        Instance = this;
    }
}
