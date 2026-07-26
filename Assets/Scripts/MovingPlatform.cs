using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Rigidbody2D rb;
    private Vector2 target;
    private Vector2 previousPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = pointB.position;
        previousPosition = rb.position;
    }

    void FixedUpdate()
    {
        previousPosition = rb.position; // capture BEFORE moving

        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, target) < 0.01f)
        {
            target = (target == (Vector2)pointA.position) ? (Vector2)pointB.position : (Vector2)pointA.position;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f) // player is standing on top
            {
                Rigidbody2D otherRb = collision.rigidbody;
                if (otherRb != null)
                {
                    Vector2 delta = rb.position - previousPosition;
                    otherRb.position += delta;
                }
                break;
            }
        }
    }
}