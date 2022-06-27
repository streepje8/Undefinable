using UnityEngine;
using Openverse.Events; 

[RequireComponent(typeof(AudioSource))]
public class SoundOnEvent : GameEventListnerGeneric
{
    private AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public override void OnEventRaised()
    {
        source.PlayOneShot(source.clip);
    }
}
