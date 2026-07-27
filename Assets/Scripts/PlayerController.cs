using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
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

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController spiritOverride;
    [SerializeField] private RuntimeAnimatorController normalController;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    private Vector2 moveVector;
    private bool isGrounded;
    private float defaultGravityScale;

    private float coyoteCounter;
    private float jumpBufferCounter;

    PlayerHealth playerhealth;

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
        playerhealth = GetComponent<PlayerHealth>();

    }

    private void Update()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        }

        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (jumpAction != null && jumpAction.WasPressedThisFrame())
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            ExecuteJump();
        }

        if (moveAction != null)
        {
            moveVector = moveAction.ReadValue<Vector2>();
        }

        if (moveVector.x > 0) facingDirection = 1f;
        else if (moveVector.x < 0) facingDirection = -1f;

        transform.localScale = new Vector3(facingDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        if (animator != null)
        {
            animator.SetBool("isMoving", Mathf.Abs(moveVector.x) > 0.01f);
        }

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckForBlackTile(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckForBlackTile(collision);
    }

    private void CheckForBlackTile(Collision2D collision)
    {
        Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
        if (tilemap == null) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector3 hitPoint = contact.point - (contact.normal * 0.05f);
            Vector3Int cellPosition = tilemap.WorldToCell(hitPoint);

            Color tileColor = tilemap.GetColor(cellPosition);

            if (tileColor.r <= 0.05f && tileColor.g <= 0.05f && tileColor.b <= 0.05f)
            {
                Debug.Log($"Stepped on black tile at position: {cellPosition}");
                playerhealth.Die();
                break;
            }
        }
    }


    public void OnWorldChanged(bool isSpiritWorld)
    {
        if (animator == null) return;
        animator.runtimeAnimatorController = isSpiritWorld ? spiritOverride : normalController;
    }

    public void TeleportToSafety()
    {
        if (SafetyTileManager.Instance != null)
        {
            Vector3 safePoint = SafetyTileManager.Instance.GetNearestSafetyTilePosition(transform.position);

            // Apply final position (safePoint already includes tile height offset)
            transform.position = safePoint + new Vector3(0f,1.2f,0f);

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        if (playerhealth != null)
        {
            playerhealth.TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            TeleportToSafety();
        }
        int layerindex = LayerMask.NameToLayer("Enemy");
        int layerindex1 = LayerMask.NameToLayer("Ending");
        if (collision.gameObject.layer == layerindex1)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
    
}

