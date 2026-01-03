using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject zombiePrefab;     // assign your zombie prefab here
    public float spawnInterval = 5f;    // time between spawns in seconds

    [Header("Spawn Area")]
    public float minX = -21f;
    public float maxX = 23f;
    public float minY = -17f;
    public float maxY = 18f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnZombie();
            timer = 0f;
        }
    }

    private void SpawnZombie()
    {
        if (zombiePrefab == null) return;

        // Random position within bounds
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector2 spawnPos = new Vector2(x, y);

        // Instantiate zombie
        Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
    }

    // Optional: visualize spawn area in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 0f);
        Gizmos.DrawWireCube(center, size);
    }
}
