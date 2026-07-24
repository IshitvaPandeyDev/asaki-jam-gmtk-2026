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
        float spawnDirection = Random.value > 0.5f ? 1f : -1f;

        Vector3 spawnPosition = playerTransform.position + new Vector3(spawnDirection * spawnDistance, 0f, 0f);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}