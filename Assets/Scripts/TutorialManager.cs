using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
}