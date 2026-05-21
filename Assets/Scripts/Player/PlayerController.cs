using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    [SerializeField] private InputActionReference movement;
    [SerializeField] private InputActionReference crouch;
    [FormerlySerializedAs("run")]
    [SerializeField] private InputActionReference sprint;
    
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
    private bool crouching;
    private bool running;

    private int health = 3;
    private float lastDamageTime = -Mathf.Infinity;
    private const float damageCooldown = 1f;
    private bool isDead;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        movement?.action?.Enable();
        crouch?.action?.Enable();
        sprint?.action?.Enable();

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveSystem.SaveGame();
            SceneManager.LoadScene("MainMenu");
            return;
        }
        if (!canMove) return;
        GetMovementInput();
        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");
        //inputDirection = new Vector3(h, 0f, v).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead)
        {
            return;
        }

        if (!collision.collider.CompareTag("Enemy"))
        {
            return;
        }

        TakeDamage(1);
    }

    private void TakeDamage(int amount)
    {
        if (Time.time - lastDamageTime < damageCooldown)
        {
            return;
        }

        lastDamageTime = Time.time;
        health = Mathf.Max(0, health - amount);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        canMove = false;

        Timer timer = FindFirstObjectByType<Timer>();
        if (timer != null)
        {
            timer.ShowLostScreen();
        }
    }

    void FixedUpdate()
    {
        if(!canMove) return;
        if (rb != null)
        {
            float currentSpeed = moveSpeed;
            if (running)
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
            if (running)
            {
                plsyerSpeed *= 2; // Running
            }
            animator.SetFloat("Speed", plsyerSpeed);
            animator.SetBool("Crouch", crouching);
        }
    }

    void GetMovementInput()
    {
        inputDirection = movement != null ? movement.action.ReadValue<Vector2>() : Vector2.zero;
        crouching = crouch != null && crouch.action.IsPressed();
        running = sprint != null && sprint.action.ReadValue<float>() > 0.1f;
        
        if(!running && Input.GetKey(KeyCode.LeftShift))
        {
            running = true;
        }
    }



    // ABILITY SYSTEM
    public void ApplyAbility(Ability ability)
    {
        // Implement ability effects on the player (e.g., modify speed, health, etc.)
        // Start a coroutine to handle the ability's duration and expiration
        StartCoroutine(ApplyAbilityEffects(ability));
    }

    bool isAbilityActive = false;
    IEnumerator ApplyAbilityEffects(Ability ability)
    {
        if (isAbilityActive)
        {
            // If an ability is already active, you can choose to either stack effects or ignore the new ability
            // For this example, we'll ignore new abilities while one is active
            yield break;
        }
        isAbilityActive = true;
        // Apply ability effects
        moveSpeed *= ability.speedModifier;
        transform.localScale *= ability.sizeModifier;
        health += ability.healthModifier;
        
        // Wait for the ability's lifetime
        yield return new WaitForSeconds(ability.abilityLifetime);
        
        // Revert ability effects after it expires
        // This is a placeholder for where you would revert the ability's effects.
        moveSpeed /= ability.speedModifier;
        transform.localScale = new Vector3(1, 1, 1); // Assuming the original scale is 1
    }
}
