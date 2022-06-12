using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Story Flag", menuName = "Custom/StoryFlag",order = 100)]
public class StoryFlag : ScriptableObject
{
    public string flagName;
    public bool isSet;
    public void SetFlag(bool value) => isSet = value; //so we can set the flag via an event listener
}
