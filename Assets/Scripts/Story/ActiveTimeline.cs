using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTimeline : MonoBehaviour
{
    public Timeline timeline;
    public TimelinePlayer player;

    private bool isPlaying = false;
    private bool isPaused = false;
    private float time = 0f;
    private List<TimelineEvent> eventqueue = new List<TimelineEvent>();
    private StoryFlag waitforFlag = null;
    
    void Update()
    {
        if (isPlaying && !isPaused)
        {
            if(eventqueue.Count < 1) //the event queue is empty, this probebly means that the timeline has finished playing.
            {
                isPlaying = false;
                return;
            }
            
            time += Time.deltaTime;

            if(time >= eventqueue[0].triggerTime) //check if the event should be triggered
            {
                //Play the event and remove it
                ExecuteTimelineEvent(eventqueue[0]);
                eventqueue.RemoveAt(0);
            }
        } else
        {
            if(isPaused)
            {
                if (waitforFlag == null || waitforFlag.isSet)
                    isPaused = false;
            } else
            {
                Destroy(this); //Timeline is finished
            }
        }
    }
    
    /// <summary>
    /// Executes a timeline event on this timeline
    /// </summary>
    /// <param name="tevent">the event to execute</param>
    public void ExecuteTimelineEvent(TimelineEvent tevent)
    {
        switch (tevent.type)
        {
            case TimelineEventType.GameEvent:
                tevent.toTrigger?.Raise();
                break;
            case TimelineEventType.FlagSetTrue:
                if (tevent.flag != null)
                    tevent.flag.SetFlag(true);
                break;
            case TimelineEventType.FlagSetFalse:
                if (tevent.flag != null)
                    tevent.flag.SetFlag(true);
                break;
            case TimelineEventType.WaitForFlag:
                if (tevent.flag != null)
                    WaitForFlag(tevent.flag);
                break;
            case TimelineEventType.SwitchTimeline:
                if (tevent.timeline != null)
                    SwitchTimeline(tevent.timeline);
                break;
            case TimelineEventType.PlayOtherTimelineAsync:
                if (tevent.timeline != null)
                    player.PlayTimeline(tevent.timeline);
                break;
            case TimelineEventType.PlayAnimation:
                if (tevent.stateName != null)
                    PlayAnimation(tevent);
                break;
        }
    }

    /// <summary>
    /// Plays an animation that a timeline event points to
    /// </summary>
    /// <param name="tevent">the event pointing to the animation</param>
    private void PlayAnimation(TimelineEvent tevent)
    {
        if (player.animators.Count > tevent.animatorID)
        {
            player.animators[tevent.animatorID].Play(tevent.stateName);
        }
        else
        {
            Debug.LogError("Timeline tried to play an animation on an animator that is not registered on the TimelinePlayer! ID: " + tevent.animatorID, this);
        }
    }

    /// <summary>
    /// Switch this active timeline to another one
    /// </summary>
    /// <param name="timeline">the timeline to switch to</param>
    public void SwitchTimeline(Timeline timeline)
    {
        player.PlayTimeline(timeline);
        isPlaying = false;
    }

    /// <summary>
    /// Pauses the current timeline until a flag is set to true
    /// </summary>
    /// <param name="flag">the flag to pause on</param>
    public void WaitForFlag(StoryFlag flag)
    {
        isPaused = true;
        waitforFlag = flag;
    }

    /// <summary>
    /// Starts playback of this timeline
    /// </summary>
    public void Play()
    {
        foreach (TimelineEvent tev in timeline.events)
        {
            eventqueue.Add(tev);
        }
        eventqueue.Sort(comparerFunction);
        isPlaying = true;   
    }
    
    /// <summary>
    /// Required to sort the timeline events
    /// </summary>
    /// <param name="x">event one</param>
    /// <param name="y">event two</param>
    /// <returns>the difference</returns>
    private int comparerFunction(TimelineEvent x, TimelineEvent y)
    {
        return x.triggerTime.CompareTo(y.triggerTime);
    }
}
