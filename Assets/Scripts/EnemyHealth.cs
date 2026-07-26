using Unity.Cinemachine;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxhealth = 1000f;
    [SerializeField] private float PlayerDamage = 50f;
    private float currenthealth;
    private PlayerHealth playerHealth;
    private CinemachineImpulseSource impulseSource;

    private void Start()
    {
        currenthealth = maxhealth;
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void TakeDamage()
    {
        currenthealth -= PlayerDamage;
        Debug.Log("Current health is " + currenthealth);
        if (currenthealth <= 0)
        {
            if (impulseSource != null)
            {
                impulseSource.GenerateImpulse();
            }
            Die();
            playerHealth.HealOnKill();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

}
