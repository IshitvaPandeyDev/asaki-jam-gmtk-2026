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
        Destroy(gameObject, lifetime);
    }


    public void Launch(Vector2 Direction)
    {
        Rigid.linearVelocity = Direction.normalized * speed;

        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        int layerindex1 = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == layerindex1)
        {
            return;
        }
        EnemyHealth enemyH = collision.GetComponent<EnemyHealth>();
        int layerindex = LayerMask.NameToLayer("Enemy");
        if (collision.gameObject.layer == layerindex)
        {
            enemyH.TakeDamage();
        }
        Destroy(gameObject);
    }
}