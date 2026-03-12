using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable { 
    public void Interact();
}
public class Interactor : MonoBehaviour
{
    [SerializeField] InputActionReference interact;

    public Transform InteractorSource;
    public float InteractRange;
    public Animator animator;

    private float interactCooldown = 0.5f;
    private float lastInteractTime = -Mathf.Infinity;

    private void Awake()
    {
        interact.action.Enable();
    }

    private void OnEnable()
    {
        interact.action.performed += Interact;
    }

    private void OnDisable()
    {
        interact.action.performed -= Interact;
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        if (Time.time - lastInteractTime < interactCooldown)
        {
            return;
        }

        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                lastInteractTime = Time.time;
                interactObj.Interact();
                if (animator != null)
                {
                    animator.SetTrigger("Interact");
                }
            }
        }
    }
}
