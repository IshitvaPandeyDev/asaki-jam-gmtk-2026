using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Cinemachine;
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int MaxHearts = 5;
    [SerializeField] private float OppDamage = 1.0f;
    [SerializeField] private float TimeStop = 0.08f;
    private float CurrentHearts;

    [Header("Drain Health Settings")]
    [SerializeField] private float DrainRatePerSecond = 0.05f;
    [SerializeField] private float HealthGainedPerKill = 0.5f;

    [Header("Invincibility Cooldown")]
    [SerializeField] private float InvincDuration = 1.0f;
    private float InvincTimer = 0f;

    [Header("UI Reference")]
    [SerializeField] private Image healthBarFill;

    CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField] ParticleSystem HurtSystem;

    [Header("Shield Power-Up")]
    private bool isShieldActive = false;

    private void Start()
    {
        CurrentHearts = MaxHearts;
        cinemachineImpulseSource = FindFirstObjectByType<CinemachineImpulseSource>();
    }

    private void Update()
    {
        DrainHealth();

        if (InvincTimer > 0)
        {
            InvincTimer -= Time.deltaTime;
        }
        UpdateHealthUI();
    }
    public void RestoreHealth(float amount)
    {
        CurrentHearts = Mathf.Min(CurrentHearts + amount, MaxHearts);
        UpdateHealthUI();
        Debug.Log("Health Restored! Current Health: " + CurrentHearts);
    }

    // Call this from the Shield power-up
    public void ActivateShield(float duration)
    {
        StartCoroutine(ShieldRoutine(duration));
    }

    private IEnumerator ShieldRoutine(float duration)
    {
        isShieldActive = true;
        Debug.Log("Shield Activated!");

        // Optional: Add visual feedback like turning sprite blue/cyan
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color originalColor = sprite != null ? sprite.color : Color.white;
        if (sprite != null) sprite.color = Color.cyan;

        yield return new WaitForSeconds(duration);

        if (sprite != null) sprite.color = originalColor;
        isShieldActive = false;
        Debug.Log("Shield Expired!");
    }
    public void TakeDamage()
    {
        if (isShieldActive || InvincTimer > 0) return;

        if (cinemachineImpulseSource == null)
        {
            cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        }

        CurrentHearts -= OppDamage;
        InvincTimer = InvincDuration;
        StartCoroutine(DamageTriggerSequence(TimeStop));
        HurtSystem.Play();
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
        GameOverManager.Instance.TriggerGameOver();
    }

    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            // Continuously converts health remaining into a 0.0 to 1.0 fill value
            healthBarFill.fillAmount = Mathf.Clamp01(CurrentHearts / MaxHearts);
        }
    }
    public IEnumerator DamageTriggerSequence(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;

        if (cinemachineImpulseSource != null)
        {
            cinemachineImpulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogWarning("CinemachineImpulseSource is NULL in Spirit World!");
        }
    }

}
