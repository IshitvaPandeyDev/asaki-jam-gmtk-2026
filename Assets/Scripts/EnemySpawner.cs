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

        // Pick random left (-1) or right (+1) offset from player
        float direction = Random.value > 0.5f ? 1f : -1f;
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

        // Since EnemySpawner is a child of Player (with local position 0,0,0),
        // transform.position IS the player's exact world position!
        Vector3 spawnPos = transform.position + new Vector3(direction * distance, 0f, 0f);

        // Instantiate in World Space so enemy moves independently of player
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        activeEnemies.Add(newEnemy);
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
}