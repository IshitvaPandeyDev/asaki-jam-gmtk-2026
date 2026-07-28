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
        Debug.Log(CurrentHearts);
        UpdateHealthUI();
    }
    public void TakeDamage()
    {
        if (InvincTimer > 0) return;

        CurrentHearts -= OppDamage;
        InvincTimer = InvincDuration;
        StartCoroutine(DamageTriggerSequence(TimeStop));

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
    }

}
