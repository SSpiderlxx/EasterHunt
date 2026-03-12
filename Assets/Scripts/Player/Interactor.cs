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
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
            }
        }
    }
}
