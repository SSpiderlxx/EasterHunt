using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference movement;
    
    [Tooltip("Movement speed in units per second.")]
    public float moveSpeed = 5f;
    public float runMultiplier = 1.5f;

    [Tooltip("Smoothly rotate to face movement direction when moving.")]
    public bool faceMovementDirection = true;
    public bool canMove = true;

    Rigidbody rb;
    //Vector3 inputDirection;
    Vector2 inputDirection;

    public Animator animator;
    private  float plsyerSpeed ; //0: idle 0.5: walk 1: run

    void Awake()
    {
        movement.action.Enable();

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if (!canMove) return;
        GetMovementInput();
        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");
        //inputDirection = new Vector3(h, 0f, v).normalized;
    }

    void FixedUpdate()
    {
        if(!canMove) return;
        if (rb != null)
        {
            float currentSpeed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed *= runMultiplier;
            }
            Vector3 targetVelocity = new Vector3(inputDirection.x, 0, inputDirection.y) * currentSpeed;
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
        }
        else
        {
            transform.position += new Vector3(inputDirection.x, 0, inputDirection.y) * (moveSpeed * Time.fixedDeltaTime);
        }

        if (faceMovementDirection && inputDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(new Vector3(inputDirection.x, 0, inputDirection.y), Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.fixedDeltaTime);
        }

        // Update animator parameters
        if (animator != null)
        {
            plsyerSpeed = inputDirection.magnitude / 2;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                plsyerSpeed *= 2; // Running
            }
            animator.SetFloat("Speed", plsyerSpeed);
        }
    }

    void GetMovementInput()
    {
        inputDirection = movement.action.ReadValue<Vector2>();

        print(inputDirection);
    }
}
