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
        foreach (InvisibleChange change in changes)
        {
            change.OnChangeCondition?.Init();
        }
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            enabled = false;
            Debug.LogWarning("EventOnInvisible Requires A Renderer To Check", this);
        }
    }

    
    void Update()
    {
        if (!rend.isVisible)
        {
            if(wasVisible)
            {
                if(changes.Count > index)
                {
                    if (changes[index].OnChangeEvent == null)
                    {
                        index++;
                        Debug.Log("BROKEN");
                        return;
                    }
                    bool canExecute = true;
                    if(changes[index].OnChangeCondition != null)
                    {
                        canExecute = changes[index].OnChangeCondition.isSet;
                        Debug.Log("NO");
                    }
                    Debug.Log(canExecute);
                    if (canExecute)
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
#if UNITY_EDITOR
        //This is not needed in the final build since scriptable objects are not saved anyway, but its nice for debugging
        foreach (InvisibleChange change in changes)
        {
            change.OnChangeCondition?.SetFlag(false);
        }
#endif
    }
}

[System.Serializable]
public struct InvisibleChange
{
    public GameEvent OnChangeEvent;
    public StoryFlag OnChangeCondition;
}