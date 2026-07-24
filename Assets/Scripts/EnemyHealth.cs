using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxhealth = 1000f;
    [SerializeField] private float PlayerDamage = 50f;
    private float currenthealth;

    private void Start()
    {
        currenthealth = maxhealth;
    }

    public void TakeDamage()
    {
        currenthealth -= PlayerDamage;
        Debug.Log("Current health is " + currenthealth);
        if (currenthealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

}
