using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private float spawnDistance = 5f;

    private void Start()
    {
        // Start the continuous spawn loop
        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (playerTransform != null && enemyPrefab != null)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        // Randomly pick left (-1) or right (+1) relative to the player
        float spawnDirection = Random.value > 0.5f ? 1f : -1f;

        // Calculate position 5 units away on the X axis
        Vector3 spawnPosition = playerTransform.position + new Vector3(spawnDirection * spawnDistance, 0f, 0f);

        // Instantiate the enemy
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}