using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject endPanel;

    private void Start()
    {
        endPanel.SetActive(false);
    }

    public void ShowEndPanel()
    {
        Time.timeScale = 0f;
        endPanel.SetActive(true);
    }

    public void MainMenu()
{
    Debug.Log("Main Menu Clicked");

    Time.timeScale = 1f;

    SceneManager.LoadScene("Main Menu");
}
}