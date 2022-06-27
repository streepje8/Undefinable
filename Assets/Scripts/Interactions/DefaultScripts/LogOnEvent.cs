using Openverse.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogOnEvent : GameEventListnerGeneric
{
    public string message;

    public override void OnEventRaised()
    {
        string replaced = message;
        replaced = replaced.Replace("{time}", Time.time.ToString());
        replaced = replaced.Replace("{position}", transform.position.ToString());
        Debug.Log(replaced);
    }
}
