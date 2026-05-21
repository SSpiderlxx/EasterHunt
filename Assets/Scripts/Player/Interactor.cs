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
    public float InteractRadius = 0.5f;
    public Animator animator;

    private float interactCooldown = 2f;
    private float lastInteractTime = -Mathf.Infinity;
    private PlayerController playerController;

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

    void Update()
    {
        if (Time.time - lastInteractTime < interactCooldown)
        {
            return;
        }
         if (playerController != null)
        {
            playerController.canMove = true;
        }
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        playerController = GetComponent<PlayerController>();
        if (Time.time - lastInteractTime < interactCooldown)
        {
            return;
        }

        Vector3 origin = InteractorSource.position;
        Vector3 direction = InteractorSource.forward;
        if (Physics.SphereCast(origin, InteractRadius, direction, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                lastInteractTime = Time.time;
                
                if (playerController != null)
                {
                    playerController.canMove = false;
                }
                interactObj.Interact();
                if (animator != null)
                {
                    animator.SetTrigger("Interact");
                }
            }
        }
    }
}
