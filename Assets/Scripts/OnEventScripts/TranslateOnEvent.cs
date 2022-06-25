using Openverse.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateOnEvent : GameEventListnerGeneric
{
    public bool isRelative = false;
    public Vector3 positionChange = Vector3.zero;
    public Vector3 rotationChange = Vector3.zero;
    public Vector3 scaleChange = Vector3.one;

    public override void OnEventRaised()
    {
        if(isRelative)
        {
            transform.Translate(positionChange);
            transform.Rotate(rotationChange);
            transform.localScale += scaleChange;
        } else
        {
            transform.position = positionChange;
            transform.rotation = Quaternion.Euler(rotationChange);
            transform.localScale = scaleChange;
        }
    }
}
