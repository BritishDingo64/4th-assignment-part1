using UnityEngine;
using System.Collections.Generic;

public class BlobSpawner : MonoBehaviour
{
    public GameObject blobPrefab;  // The Blob AI prefab to spawn
    public Vector3 spawnAreaSize = new Vector3(10f, 0, 10f);  // Area for random spawn positions
    public float spawnInterval = 60f;  // Time interval (in seconds) between spawns
    public int maxBlobs = 5;  // Maximum number of blobs in the game

    private List<GameObject> activeBlobs = new List<GameObject>();  // List to track spawned blobs
    private float spawnTimer = 0f;  // Timer to track spawn intervals

    private void Update()
    {
        // Increment the timer
        spawnTimer += Time.deltaTime;

        // Check if it's time to spawn a blob and the maximum count hasn't been reached
        if (spawnTimer >= spawnInterval && activeBlobs.Count < maxBlobs)
        {
            SpawnBlob();
            spawnTimer = 0f;  // Reset the timer
        }

        // Clean up any destroyed blobs from the active list
        activeBlobs.RemoveAll(blob => blob == null);
    }

    private void SpawnBlob()
    {
        // Generate a random position within the spawn area
        Vector3 spawnPosition = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            0,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        // Instantiate the blob and add it to the active list
        GameObject newBlob = Instantiate(blobPrefab, spawnPosition, Quaternion.identity);
        activeBlobs.Add(newBlob);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the spawn area in the Scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
