using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius = 0.2f;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction jumpAction;

    private Vector2 moveVector;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void Update()
    {
        // 1. Draw an invisible circle at the player's feet to check for the Ground layer
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 2. Read movement input
        moveVector = moveAction.ReadValue<Vector2>();

        // 3. Jump ONLY if the button is pressed AND the player is touching the ground
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        // 4. Apply horizontal movement to physics velocity
        rb.linearVelocity = new Vector2(moveVector.x * moveSpeed, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        // This draws a green circle in your Scene view so you can visually see your ground check!
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}