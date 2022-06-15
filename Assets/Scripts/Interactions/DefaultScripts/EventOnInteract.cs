using Openverse.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOnInteract : MonoBehaviour,IInteractable
{
    public GameEvent onClick;
    public GameEvent hoverStart;
    public GameEvent hoverEnd;

    public void OnInteract() 
    {
        onClick?.Raise();
    }

    private int hoverState = 0;

    public void WhileHover() 
    {
        if (hoverState < 1)
            hoverStart?.Raise();
        hoverState = 3;
    }

    private void Update()
    {
        if(hoverState > 0 && (hoverState - 1) < 1)
            hoverEnd?.Raise();
        if (hoverState > 0)
            hoverState--;
    }
}
