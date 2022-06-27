using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogOnInteract : MonoBehaviour, IInteractable
{
    public bool sendMessageOnHoverToo = false;
    public string message = "";

    public void OnInteract()
    {
        Debug.Log(message);
    }

    public void WhileHover()
    {
        if (sendMessageOnHoverToo)
            Debug.Log(message);
    }
}
