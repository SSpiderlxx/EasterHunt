using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable { 
    public void Interact();
}
public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    public Animator animator;

    private float interactCooldown = 0.5f;
    private float lastInteractTime = -Mathf.Infinity;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time - lastInteractTime >= interactCooldown)
        {
            lastInteractTime = Time.time;
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if (Physics.Raycast(r,out RaycastHit hitInfo, InteractRange)) { 
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                { 
                    interactObj.Interact();
                    if (animator != null)
                    {
                        animator.SetTrigger("Interact");
                    }
                }
            }
        }
    }
}
