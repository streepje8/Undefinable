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
            if(eventqueue.Count < 1)
            {
                isPlaying = false;
                return;
            }
            time += Time.deltaTime;
            if(time >= eventqueue[0].triggerTime)
            {
                TimelineEvent tevent = eventqueue[0];
                switch(tevent.type)
                {
                    case TimelineEventType.GameEvent:
                        tevent.toTrigger?.Raise();
                        break;
                    case TimelineEventType.FlagSetTrue:
                        if(tevent.flag != null)
                        {
                            tevent.flag.SetFlag(true);
                        }
                        break;
                    case TimelineEventType.FlagSetFalse:
                        if (tevent.flag != null)
                        {
                            tevent.flag.SetFlag(true);
                        }
                        break;
                    case TimelineEventType.WaitForFlag:
                        if (tevent.flag != null) {
                            isPaused = true;
                            waitforFlag = tevent.flag;
                        }
                        break;
                    case TimelineEventType.SwitchTimeline:
                        if(tevent.timeline != null)
                        {
                            player.PlayTimeline(tevent.timeline);
                            isPlaying = false;
                        }
                        break;
                    case TimelineEventType.PlayOtherTimelineAsync:
                        if (tevent.timeline != null)
                        {
                            player.PlayTimeline(tevent.timeline);
                        }
                        break;
                    case TimelineEventType.PlayAnimation:
                        if (tevent.stateName != null)
                        {
                            if(player.animators.Count > tevent.animatorID)
                            {
                                player.animators[tevent.animatorID].Play(tevent.stateName);
                            } else
                            {
                                Debug.LogError("Timeline tried to play an animation on an animator that is not registered on the TimelinePlayer! ID: " + tevent.animatorID, this);
                            }
                        }
                        break;
                }
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

    internal void Play()
    {
        foreach (TimelineEvent tev in timeline.events)
        {
            eventqueue.Add(tev);
        }
        eventqueue.Sort(comparerFunction);
        isPlaying = true;   
    }

    public int comparerFunction(TimelineEvent x, TimelineEvent y)
    {
        return x.triggerTime.CompareTo(y.triggerTime);
    }
}
