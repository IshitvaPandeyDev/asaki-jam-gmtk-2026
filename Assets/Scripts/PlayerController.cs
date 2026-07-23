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

        // Safe check for InputSystem.actions
        if (InputSystem.actions != null)
        {
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
        }
        else
        {
            Debug.LogWarning("InputSystem.actions is null! Make sure Default Input Actions are set in Project Settings.");
        }
    }

    private void Update()
    {
        // Safe check before accessing groundCheck.position
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        }

        // Safe check before reading input values
        if (moveAction != null)
        {
            moveVector = moveAction.ReadValue<Vector2>();
        }

        // Jump logic
        if (jumpAction != null && jumpAction.WasPressedThisFrame() && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveVector.x * moveSpeed, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}