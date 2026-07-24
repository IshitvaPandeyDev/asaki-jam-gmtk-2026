using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Variable Jump Feel")]
    [SerializeField] private float fallGravityMultiplier = 2.5f; 
    [SerializeField] private float lowJumpGravityMultiplier = 2f;  

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius = 0.2f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    private float facingDirection = 1f; 
    Projectile Proj;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    private Vector2 moveVector;
    private bool isGrounded;
    private float defaultGravityScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb.gravityScale;

        if (InputSystem.actions != null)
        {
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
            attackAction = InputSystem.actions.FindAction("Attack");
        }
    }

    private void Update()
    {
        // Shoot check
        if (attackAction != null && attackAction.WasPressedThisFrame())
        {
            Shoot();
        }
        // 1. Ground check
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        }

        // 2. Read horizontal input
        if (moveAction != null)
        {
            moveVector = moveAction.ReadValue<Vector2>();
        }

        // 3. Jump initiation
        if (jumpAction != null && jumpAction.WasPressedThisFrame() && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 4. Variable Jump Height Logic
        ApplyVariableGravity();
        if (moveVector.x > 0)
        {
            facingDirection = 1f;
            // spriteRenderer.flipX = false; // If using SpriteRenderer flip
        }
        else if (moveVector.x < 0)
        {
            facingDirection = -1f;
            // spriteRenderer.flipX = true; // If using SpriteRenderer flip
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveVector.x * moveSpeed, rb.linearVelocity.y);
    }

    private void ApplyVariableGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = defaultGravityScale * fallGravityMultiplier;
        }
        else if (rb.linearVelocity.y > 0 && jumpAction != null && !jumpAction.IsPressed())
        {
            rb.gravityScale = defaultGravityScale * lowJumpGravityMultiplier;
        }
        else
        {
            rb.gravityScale = defaultGravityScale;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        // Determine facing direction based on localScale.x (+1 for Right, -1 for Left)
        
        Vector2 shootDirection = new Vector2(facingDirection, 0f);

        // Spawn bullet at FirePoint position
        GameObject bulletObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Launch it
        Projectile projectile = bulletObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Launch(shootDirection);
        }
    }

}