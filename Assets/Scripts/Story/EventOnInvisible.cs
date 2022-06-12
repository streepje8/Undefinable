using Openverse.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class EventOnInvisible : MonoBehaviour
{
    public List<InvisibleChange> changes = new List<InvisibleChange>();

    private int index = 0;
    private Renderer rend = null;
    private bool wasVisible = false;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            enabled = false;
            Debug.LogWarning("EventOnInvisible Requires A Renderer To Check", this);
        }
    }

    
    void Update()
    {
        if(!rend.isVisible)
        {
            if(wasVisible)
            {
                if(changes.Count > index)
                {
                    if (changes[index].OnChangeEvent == null || changes[index].OnChangeCondition.isSet)
                    {
                        changes[index].OnChangeEvent?.Raise();
                        index++;
                    }
                }
                wasVisible = false;
            }
        } else
        {
            wasVisible = true;
        }
    }

    private void OnApplicationQuit()
    {
        //This is not needed in the final build since scriptable objects are not saved anyway, but its nice for debugging
        foreach (InvisibleChange change in changes)
            change.OnChangeCondition.isSet = false;
    }
}

[System.Serializable]
public struct InvisibleChange
{
    public GameEvent OnChangeEvent;
    public StoryFlag OnChangeCondition;
}