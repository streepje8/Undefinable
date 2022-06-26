using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SluisSysteem : Singleton<SluisSysteem>
{
    public int Cycle = 0;

    [Header("Doors")]
    public Door firstDoor;
    public Door secondDoor;
    public Door huiskamerDoor;

    [Header("Portals")]
    public Portal portalHuiskamer;
    public Portal portalGangA;
    public Portal portalGangB;

    [Header("Flags")]
    public List<StoryFlag> flags = new List<StoryFlag>();

    private bool state = false;

    private void Awake()
    {
        foreach(StoryFlag f in flags)
        {
            f.Init();
        }
    }

    public void InteractWithFirstDoor()
    {
        state = true;
        firstDoor.SetDoorState(true);
        secondDoor.SetDoorState(false);
        portalHuiskamer.isAbleToTeleport = true;
        portalGangB.isAbleToTeleport = true;
        portalGangA.isAbleToTeleport = false;
    }

    public void InteractWithSecondDoor()
    {
        if(state)
        {
            state = false;
            firstDoor.SetDoorState(false);
            huiskamerDoor.SetDoorState(false);
            secondDoor.SetDoorState(true);
            portalHuiskamer.isAbleToTeleport = false;
            portalGangB.isAbleToTeleport = false;
            portalGangA.isAbleToTeleport = true;
            Cycle++;
            flags[Cycle - 1].SetFlag(true);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
