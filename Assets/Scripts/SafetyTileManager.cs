using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SafetyTileManager : MonoBehaviour
{
    public static SafetyTileManager Instance { get; private set; }

    [SerializeField] private Tilemap safetyTilemap;

    // Cache list storing world positions of all safety tiles
    private List<Vector3> cachedSafetyPositions = new List<Vector3>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        CacheSafetyTiles();
    }

    private void CacheSafetyTiles()
    {
        if (safetyTilemap == null)
        {
            Debug.LogError("Safety Tilemap is not assigned in SafetyTileManager!");
            return;
        }

        cachedSafetyPositions.Clear();
        BoundsInt bounds = safetyTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (safetyTilemap.HasTile(pos))
            {
                // Convert tile grid coordinate to exact world position (center of the tile)
                Vector3 worldPos = safetyTilemap.GetCellCenterWorld(pos);
                cachedSafetyPositions.Add(worldPos);
            }
        }

        Debug.Log($"Cached {cachedSafetyPositions.Count} safety tile positions.");
    }

    /// <summary>
    /// Returns the world position of the safety tile closest to currentPosition.
    /// </summary>
    public Vector3 GetNearestSafetyTilePosition(Vector3 currentPosition)
    {
        if (cachedSafetyPositions.Count == 0)
        {
            Debug.LogWarning("No safety tiles cached!");
            return currentPosition;
        }

        Vector3 nearestTile = cachedSafetyPositions[0];
        float minDistanceSqr = (currentPosition - nearestTile).sqrMagnitude;

        for (int i = 1; i < cachedSafetyPositions.Count; i++)
        {
            float distSqr = (currentPosition - cachedSafetyPositions[i]).sqrMagnitude;
            if (distSqr < minDistanceSqr)
            {
                minDistanceSqr = distSqr;
                nearestTile = cachedSafetyPositions[i];
            }
        }

        // Add a slight vertical offset (e.g., +0.5f on Y) so player doesn't spawn embedded inside the tile surface
        return nearestTile + new Vector3(0, 0.5f, 0);
    }
}