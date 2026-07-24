using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float deceleration = 50f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.15f;

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

    private float coyoteCounter;
    private float jumpBufferCounter;

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
        // Ground Check
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        }

        // Coyote Time Logic
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        // Jump Buffer Logic
        if (jumpAction != null && jumpAction.WasPressedThisFrame())
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Execute Jump if both conditions are met
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            ExecuteJump();
        }

        // Input Reading & Facing Direction
        if (moveAction != null)
        {
            moveVector = moveAction.ReadValue<Vector2>();
        }

        if (moveVector.x > 0) facingDirection = 1f;
        else if (moveVector.x < 0) facingDirection = -1f;

        // Shooting
        if (attackAction != null && attackAction.WasPressedThisFrame())
        {
            Shoot();
        }

        ApplyVariableGravity();
    }

    private void ExecuteJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpBufferCounter = 0f;
    }
    private void FixedUpdate()
    {
        float targetSpeed = moveVector.x * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = speedDiff * accelRate;

        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
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

        
        Vector2 shootDirection = new Vector2(facingDirection, 0f);

        GameObject bulletObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Projectile projectile = bulletObj.GetComponent<Projectile>();
        if (projectile != null)
        { 
            projectile.Launch(shootDirection);
        }
    }

}