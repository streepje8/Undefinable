using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public PortalCamera mainCamera;

    private void Awake()
    {
        Instance = this;
    }
}
