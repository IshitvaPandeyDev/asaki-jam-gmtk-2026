using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float spawnInterval = 10f;

    [Header("Near-Player Spawn Settings")]
    [SerializeField] private float minSpawnDistance = 3f;  // Keeps enemies from spawning directly on top of the player
    [SerializeField] private float maxSpawnDistance = 7f;  // Keeps enemies close by
    [SerializeField] private LayerMask groundLayer;        // Assign your Ground/Tilemap layer in Inspector

    private Coroutine spawnCoroutine;

    private void OnEnable()
    {
        // Auto-find player if reference is missing
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnEnemyRoutine());
    }

    private void OnDisable()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (playerTransform != null && enemyPrefab != null)
            {
                SpawnEnemyNearPlayer();
            }
        }
    }

    private void SpawnEnemyNearPlayer()
    {
        // 1. Pick a random direction (Left or Right of player)
        float spawnDirection = Random.value > 0.5f ? 1f : -1f;

        // 2. Pick a random distance within min/max bounds
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        // 3. Compute base spawn position relative to player
        Vector3 targetSpawnPos = playerTransform.position + new Vector3(spawnDirection * randomDistance, 0f, 0f);

        // 4. Raycast down from above the target spot to find the actual floor height
        Vector3 rayStart = targetSpawnPos + Vector3.up * 3f;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, 6f, groundLayer);

        if (hit.collider != null)
        {
            // Spawn slightly above the ground surface so the enemy doesn't get stuck in floor tiles
            Vector3 safeGroundPos = hit.point + new Vector2(0f, 0.5f);
            Instantiate(enemyPrefab, safeGroundPos, Quaternion.identity, transform);
            Debug.Log("Enemy spawned near player on valid ground!");
        }
        else
        {
            // Fallback: If no floor was hit below, spawn at target position directly
            Instantiate(enemyPrefab, targetSpawnPos, Quaternion.identity, transform);
            Debug.LogWarning("No ground hit under spawn point; spawned using raw position.");
        }
    }
}