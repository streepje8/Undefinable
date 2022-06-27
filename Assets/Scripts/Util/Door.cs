using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    public Vector3 rotationOffset = Vector3.zero;
    public int openAngle = 145;
    public bool interactable = true;
    private int rotation = 0;
    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();        
    }

    public void ToggleOpenClosed()
    {
        if (interactable)
        {
            rotation = rotation < 10 ? openAngle : 0;
            source.PlayOneShot(source.clip);
        }
    }

    public void SetDoorState(bool state)
    {
        rotation = state ? openAngle : 0;
        if(source.clip != null)
            source.PlayOneShot(source.clip);
    }

    public void SetInteractable(bool state)
    {
        interactable = state;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotationOffset + Vector3.up * rotation), 3f * Time.deltaTime);
    }
}
