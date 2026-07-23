using UnityEngine;
using UnityEngine.WSA;

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
    public void Launcher(Vector2 Direction)
    {
        Rigid.linearVelocity = Direction.normalized * speed;
        float angle =  Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
        transform. rotation = Quaternion.Euler(0,0,angle);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int LayerIndex = LayerMask.NameToLayer("Enemy");
        if (collision.gameObject.layer == LayerIndex)
        {
           
        }
    }
}
