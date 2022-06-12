using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelinePlayer : MonoBehaviour
{
    public bool playOnAwake = true;
    public Timeline rootTimeline;
    public List<Animator> animators = new List<Animator>();

    private bool isPlaying = false;
    private void Awake()
    {
        if (playOnAwake)
            Play();
    }

    void Play()
    {
        if(rootTimeline == null)
        {
            Debug.LogError("Tried to play a timeline, but it was null!", this);
            return;
        }
        if(!isPlaying)
        {
            isPlaying = true;
            PlayTimeline(rootTimeline);
        }
    }

    public void PlayTimeline(Timeline timeline)
    {
        ActiveTimeline at = gameObject.AddComponent<ActiveTimeline>();
        at.timeline = timeline;
        at.player = this;
        at.Play();
    }
}
