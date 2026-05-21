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
    private Vector3 lastHitPoint;
    private bool lastHit;

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

        Debug.DrawLine(origin, origin + direction * InteractRange, Color.yellow, 1f);
        Debug.Log($"Interactor.SphereCast -> origin: {origin}, dir: {direction}, radius: {InteractRadius}, range: {InteractRange}");

        RaycastHit[] hits = Physics.SphereCastAll(origin, InteractRadius, direction, InteractRange);
        if (hits != null && hits.Length > 0)
        {
            lastHit = true;
            lastHitPoint = hits[0].point;
            Debug.Log($"Interactor.SphereCastAll -> {hits.Length} hits");

            foreach (var hitInfo in hits)
            {
                Debug.Log($"Interactor hit: {hitInfo.collider.gameObject.name} at distance {hitInfo.distance} (point {hitInfo.point})");

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
                    break; // stop after the first interactable
                }
            }
        }
        else
        {
            lastHit = false;
            Debug.Log("Interactor: no interactable hit by SphereCastAll");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (InteractorSource == null) return;

        // Draw the cast sphere at the origin
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(InteractorSource.position, InteractRadius);

        // Draw the cast path
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(InteractorSource.position, InteractorSource.position + InteractorSource.forward * InteractRange);

        // If we hit something recently, draw the hit point
        if (lastHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(lastHitPoint, 0.15f);
        }
    }
}
