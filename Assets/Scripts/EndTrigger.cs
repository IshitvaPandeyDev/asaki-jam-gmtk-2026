using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private EndPanelManager endPanelManager;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            endPanelManager.ShowEndPanel();
        }
    }
}