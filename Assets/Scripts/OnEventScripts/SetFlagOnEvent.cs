using Openverse.Events;
public class SetFlagOnEvent : GameEventListnerGeneric
{
    public StoryFlag flag;
    public bool value;
    private void Start()
    {
        flag.Init();
    }

    public override void OnEventRaised()
    {
        flag.SetFlag(value);
    }
}
