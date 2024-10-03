using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Reference to the enemy prefab to spawn
    public GameObject Enemy;

    // List of spawn points
    public Transform[] spawnPoints;

    // Time between spawns
    public float spawnDelay = 5f;
    private float spawnTimer;

    // Number of enemies to spawn each time
    public int enemiesPerSpawn = 1;

    void Start()
    {
        // Start the timer
        spawnTimer = spawnDelay;
    }

    void Update()
    {
        // Countdown the timer
        spawnTimer -= Time.deltaTime;

        // When the timer hits zero, spawn enemies
        if (spawnTimer <= 0)
        {
            SpawnEnemies();
            spawnTimer = spawnDelay; // Reset the timer
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            // Choose a random spawn point from the array
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Instantiate the enemy prefab at the chosen spawn point
            Instantiate(Enemy, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
