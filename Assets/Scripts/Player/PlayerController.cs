using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("Movement speed in units per second.")]
    public float moveSpeed = 5f;
    public float runMultiplier = 1.5f;

    [Tooltip("Smoothly rotate to face movement direction when moving.")]
    public bool faceMovementDirection = true;

    Rigidbody rb;
    Vector3 inputDirection;

    public Animator animator;
    private  float plsyerSpeed ; //0: idle 0.5: walk 1: run

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector3(h, 0f, v).normalized;
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            float currentSpeed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed *= runMultiplier;
            }
            Vector3 targetVelocity = inputDirection * currentSpeed;
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
        }
        else
        {
            transform.position += inputDirection * (moveSpeed * Time.fixedDeltaTime);
        }

        if (faceMovementDirection && inputDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.fixedDeltaTime);
        }

        // Update animator parameters
        if (animator != null)
        {
            plsyerSpeed = inputDirection.magnitude / 2;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                plsyerSpeed = 1f; // Running
            }
            animator.SetFloat("Speed", plsyerSpeed);
        }
    }
}
