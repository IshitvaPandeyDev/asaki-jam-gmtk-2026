using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    [Header("World References")]
    [SerializeField] private GameObject normalWorld;
    [SerializeField] private GameObject spiritWorld;
    [SerializeField] private Tilemap spiritTilemap;

    [Header("Timing Settings")]
    [SerializeField] private float switchTime = 15f;
    [SerializeField] private float DecayInterval = 0.5f;
    [SerializeField] private int TilesPerDecay = 2; 

    private List<Vector3Int> activeTiles = new List<Vector3Int>();
    private bool isSpiritWorldActive = false;
    private float timer;
    CinemachineCamera cinemachineCamera;

    public float Timer
    {
        get { return timer; }
    }

    void Start()
    {
        timer = switchTime;
        isSpiritWorldActive = false;

        normalWorld.SetActive(true);
        spiritWorld.SetActive(false);

        CacheTiles();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SwitchWorld();
            timer = switchTime;
        }
    }

    void SwitchWorld()
    {
        isSpiritWorldActive = !isSpiritWorldActive;

        normalWorld.SetActive(!isSpiritWorldActive);
        spiritWorld.SetActive(isSpiritWorldActive);

        if (isSpiritWorldActive)
        {
            StartCoroutine(StartSlowDestruction());
        }
        var confiner = FindFirstObjectByType<Unity.Cinemachine.CinemachineConfiner2D>();
        if (confiner != null)
        {
            confiner.InvalidateBoundingShapeCache();
        }
    }

    private void CacheTiles()
    {
        if (spiritTilemap == null)
        {
            Debug.LogError("Spirit Tilemap is NOT assigned in the WorldManager Inspector!");
            return;
        }

        activeTiles.Clear();
        BoundsInt Bounds = spiritTilemap.cellBounds;

        for (int x = Bounds.xMin; x < Bounds.xMax; x++)
        {
            for (int y = Bounds.yMin; y < Bounds.yMax; y++)
            {
                Vector3Int localplace = new Vector3Int(x, y, 0);
                if (spiritTilemap.HasTile(localplace))
                {
                    activeTiles.Add(localplace);
                }
            }
        }

        activeTiles.Sort((a, b) => a.x.CompareTo(b.x));

        Debug.Log($"Cached {activeTiles.Count} tiles from Spirit Tilemap.");
    }

    private IEnumerator StartSlowDestruction()
    {
        while (isSpiritWorldActive && activeTiles.Count > 0)
        {
            yield return new WaitForSeconds(DecayInterval);

            for (int i = 0; i < TilesPerDecay; i++)
            {
                if (activeTiles.Count == 0) break;

                Vector3Int targetCell = activeTiles[0];

                spiritTilemap.SetTileFlags(targetCell, TileFlags.None);

                spiritTilemap.SetColor(targetCell, Color.black);

                activeTiles.RemoveAt(0);
            }
        }
    }
}