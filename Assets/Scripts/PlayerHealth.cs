using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int MaxHearts = 5;
    [SerializeField] private float OppDamage = 1.0f;
    private float CurrentHearts;

    [Header("Drain Health Settings")]
    [SerializeField] private float DrainRatePerSecond = 0.05f;
    [SerializeField] private float HealthGainedPerKill = 0.5f;

    [Header("Invincibility Cooldown")]
    [SerializeField] private float InvincDuration = 1.0f;
    private float InvincTimer = 0f;

    private void Start()
    {
        CurrentHearts = MaxHearts;
    }

    private void Update()
    {
        DrainHealth();

        if (InvincTimer > 0)
        {
            InvincTimer -= Time.deltaTime;
        }
        Debug.Log(CurrentHearts);
    }
    public void TakeDamage()
    {
        if (InvincTimer > 0) return;

        CurrentHearts -= OppDamage;
        InvincTimer = InvincDuration;

        if (CurrentHearts <= 0)
        {
            Die();
        }
        Debug.Log(CurrentHearts);
    }

    private void DrainHealth()
    {
        CurrentHearts -= DrainRatePerSecond * Time.deltaTime;

        if (CurrentHearts <= 0)
        {
            Die();
        }
    }

    public void HealOnKill()
    {
        CurrentHearts = Mathf.Min(CurrentHearts + HealthGainedPerKill, MaxHearts);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layerindex = LayerMask.NameToLayer("Enemy");
        if (collision.gameObject.layer == layerindex)
        {
            TakeDamage();
            Debug.Log("boom");
        }
    }
    public void Die()
    {
        gameObject.SetActive(false);
        GameOverManager.Instance.TriggerGameOver();
    }
}
