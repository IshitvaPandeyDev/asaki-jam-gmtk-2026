using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    public TextMeshProUGUI timerText;
    public WorldManager worldManager;

    public float timer
    {
    get { return timer; }
    }

void Update()
{
    timerText.text = worldManager.Timer.ToString("F2");
}




}
