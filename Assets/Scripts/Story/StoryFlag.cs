using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Story Flag", menuName = "Custom/StoryFlag",order = 100)]
public class StoryFlag : ScriptableObject
{
    public string flagName;
    private bool _flag;
    public bool isSet {
        get
        {
            if (!isInitialized)
                throw new System.Exception("Story flag " + flagName + " was read before it was initalized!");
            return _flag;
        }
    }
    private static bool isInitialized = false;
    public void Init()
    {
        _flag = false;
        isInitialized = true;
    }
    public void SetFlag(bool value) => _flag = value; //so we can set the flag via an event listener
}
