using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundOnInteract : MonoBehaviour, IInteractable
{
    private AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    

    public void OnInteract()
    {
        source.PlayOneShot(source.clip);
    }

    public void WhileHover() {}
}
