using Openverse.Events;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Story Flag", menuName = "Custom/Timeline", order = 100)]
public class Timeline : ScriptableObject
{
    public List<TimelineEvent> events = new List<TimelineEvent>();
}

public enum TimelineEventType
{
    GameEvent,
    FlagSetTrue,
    FlagSetFalse,
    WaitForFlag,
    SwitchTimeline,
    PlayOtherTimelineAsync,
    PlayAnimation
}

[System.Serializable]
public struct TimelineEvent
{
    public float triggerTime;
    public TimelineEventType type;
    public GameEvent toTrigger;
    public StoryFlag flag;
    public Timeline timeline;
    public int animatorID;
    public string stateName;
}
