using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 15.0f;
    [SerializeField] private float lifetime = 3.0f;

    private Rigidbody2D Rigid;

    private void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Destroy the bullet after a few seconds so it doesn't clutter memory
        Destroy(gameObject, lifetime);
    }

    // We DELETED the Update() method so it doesn't fight the physics system!

    public void Launch(Vector2 Direction)
    {
        // 1. Give it physics velocity
        Rigid.linearVelocity = Direction.normalized * speed;

        // 2. Rotate it correctly (Notice it is Y first, then X!)
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Remember to ignore the player so they don't shoot themselves!
        if (collision.CompareTag("Player")) return;

        // Destroy projectile when it hits a wall/enemy
        Destroy(gameObject);
    }
}