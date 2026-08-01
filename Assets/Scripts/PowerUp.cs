using UnityEngine;

public enum PowerUpType
{
    Shield,
    SpeedBoost,
    HealthRestore
}

public class PowerUp : MonoBehaviour
{
    [Header("Power-Up Type")]
    [SerializeField] private PowerUpType type;

    [Header("Values")]
    [SerializeField] private float duration = 5f;       
    [SerializeField] private float speedMultiplier = 2f;  
    [SerializeField] private float healAmount = 2f;         

    [Header("Optional Pickup VFX")]
    [SerializeField] private GameObject pickupEffectPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyPowerUp(collision.gameObject);

            if (pickupEffectPrefab != null)
            {
                Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject); // Collect item
        }
    }

    private void ApplyPowerUp(GameObject player)
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        PlayerController controller = player.GetComponent<PlayerController>();

        switch (type)
        {
            case PowerUpType.Shield:
                if (health != null) health.ActivateShield(3f); // Shield for 3 seconds
                break;

            case PowerUpType.SpeedBoost:
                if (controller != null) controller.ActivateSpeedBoost(speedMultiplier, 5f); // Double speed for 5 seconds
                break;

            case PowerUpType.HealthRestore:
                if (health != null) health.RestoreHealth(healAmount);
                break;
        }
    }
}