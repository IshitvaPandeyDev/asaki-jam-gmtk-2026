using UnityEngine;

public class EnemyFollowAvoidance : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float obstacleCheckDistance = 1f;
    [SerializeField] private LayerMask obstacleLayer;

    private Rigidbody2D myRigidBody;
    private Transform playerTransform;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // 1. Get direction toward player
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // 2. Cast a ray forward to check for tile colliders
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, obstacleCheckDistance, obstacleLayer);

        Vector2 moveDirection = directionToPlayer;

        // 3. If an obstacle tile is in the direct path, steer up/down to bypass it
        if (hit.collider != null)
        {
            // Perpendicular steering vector
            moveDirection += new Vector2(-directionToPlayer.y, directionToPlayer.x);
            moveDirection.Normalize();
        }

        // 4. Apply velocity
        myRigidBody.linearVelocity = moveDirection * moveSpeed;

        // 5. Flip sprite facing direction
        if (Mathf.Abs(myRigidBody.linearVelocity.x) > 0.1f)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.linearVelocity.x), 1f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the obstacle detection ray in the Scene view
        if (playerTransform != null)
        {
            Gizmos.color = Color.red;
            Vector2 dir = (playerTransform.position - transform.position).normalized;
            Gizmos.DrawRay(transform.position, dir * obstacleCheckDistance);
        }
    }
}