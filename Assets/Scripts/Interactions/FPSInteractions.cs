using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInteractions : MonoBehaviour
{
    public LayerMask interactableLayer;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2.5f, interactableLayer))
        {
            IInteractable[] interactables = hit.collider.gameObject.GetComponents<IInteractable>();
            if (interactables.Length > 0)
            {
                for (int i = 0; i < interactables.Length; i++)
                {
                    IInteractable interactable = interactables[i];
                    interactable.WhileHover();
                    if (Input.GetMouseButtonDown(0))
                    {
                        interactable.OnInteract();
                    }
                }
            }
        }
    }
}
