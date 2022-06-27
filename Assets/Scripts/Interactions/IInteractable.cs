using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// Gets called repeatitly while game object is hovered
    /// </summary>
    void WhileHover();
    /// <summary>
    /// Gets called once for every time the game object is clicked
    /// </summary>
    void OnInteract();
}
