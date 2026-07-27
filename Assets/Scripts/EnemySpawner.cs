using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 4f;

    [Header("Spawn Offset From Player")]
    [SerializeField] private float minSpawnDistance = 3f;  // Min distance to left/right
    [SerializeField] private float maxSpawnDistance = 6f;  // Max distance to left/right

    [Header("Invalid Spawn Prevention")]
    [SerializeField] private LayerMask invalidSpawnLayers; // Assign Ground & Safety Tile layers in Inspector
    [SerializeField] private float checkRadius = 0.5f;     // Size of the check area (matches enemy size)
    [SerializeField] private int maxSpawnAttempts = 10;    // Number of retry attempts before giving up

    private Coroutine spawnCoroutine;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void OnEnable()
    {
        // 1. Immediately spawn a fresh enemy near player when entering Normal World
        SpawnEnemyNearPlayer();

        // 2. Restart the spawning timer loop
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnEnemyRoutine());
    }

    private void OnDisable()
    {
        // 1. Stop the spawner timer loop when entering Spirit World
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // 2. Destroy all active enemies so none linger in the Spirit World
        DestroyAllEnemies();
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemyNearPlayer();
        }
    }

    private void SpawnEnemyNearPlayer()
    {
        if (enemyPrefab == null) return;

        // Clean up references to enemies that were killed by the player
        activeEnemies.RemoveAll(enemy => enemy == null);

        Vector3 validSpawnPos = Vector3.zero;
        bool foundValidSpot = false;

        // Try a few times to find a position that isn't inside ground or safety tiles
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            float direction = Random.value > 0.5f ? 1f : -1f;
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

            Vector3 candidatePos = transform.position + new Vector3(direction * distance, 0f, 0f);

            // Check if the candidate position overlaps with forbidden layers (Ground/Safety)
            Collider2D hit = Physics2D.OverlapCircle(candidatePos, checkRadius, invalidSpawnLayers);

            if (hit == null)
            {
                validSpawnPos = candidatePos;
                foundValidSpot = true;
                break; // Found a clean spot!
            }
        }

        // Only spawn if a valid spot was found
        if (foundValidSpot)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, validSpawnPos, Quaternion.identity);
            activeEnemies.Add(newEnemy);
        }
    }

    public void DestroyAllEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] != null)
            {
                Destroy(activeEnemies[i]);
            }
        }
        activeEnemies.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the check area in Scene View
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minSpawnDistance);
        Gizmos.DrawWireSphere(transform.position, maxSpawnDistance);
    }
}