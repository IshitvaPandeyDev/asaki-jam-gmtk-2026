using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject normalWorld;
    public GameObject spiritWorld;

    public float switchTime = 20f;

    private float timer;

    public float Timer
{
    get { return timer; }
}

    private bool isSpiritWorld;
    
void Start()
{
    timer = switchTime;

    isSpiritWorld = false;

    normalWorld.SetActive(true);
    spiritWorld.SetActive(false);
}
    

void Update()
{
    timer -= Time.deltaTime;

    if(timer <= 0)
    {
        SwitchWorld();

        timer = switchTime;
    }
}

void SwitchWorld()
{
    isSpiritWorld = !isSpiritWorld;

    normalWorld.SetActive(!isSpiritWorld);
    spiritWorld.SetActive(isSpiritWorld);
}

}


