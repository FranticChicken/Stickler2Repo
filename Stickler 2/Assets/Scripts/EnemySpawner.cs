using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Reference to the enemy prefab to spawn
    public GameObject Enemy;
    public GameObject Enemy2;
    public GameObject Enemy3;

    // List of spawn points
    public Transform[] spawnPoints;

    // Time between spawns
    public float spawnDelay;
    private float spawnTimer;

    // Number of enemies to spawn each time
    public int enemiesPerSpawn = 1;

    //Wave Controller stuff
    public WavesController waveControllerScript;
    int waveNum = 1;
    int numOfEnemiesSpawned;

    //dialogue Controller stuff
    public DialogueManager dialogueManager;

    private void Awake()
    {
        numOfEnemiesSpawned = 0;
    }

    void Start()
    {
        // Start the timer
        spawnTimer = spawnDelay;
    }

    void Update()
    {
        //get spawnDelay depending on wave
        spawnDelay = waveControllerScript.spawnDelay;

        // Countdown the timer
        spawnTimer -= Time.deltaTime;

        // When the timer hits zero, spawn enemies
        if (spawnTimer <= 0)
        {
            SpawnEnemies();
            spawnTimer = spawnDelay; // Reset the timer
        }

        if(numOfEnemiesSpawned == waveControllerScript.numberOfEnemies && waveControllerScript.numberOfEnemies == waveControllerScript.spidersKilled)
        {
            //waveNum++;
            numOfEnemiesSpawned = 0;
        }
    }

    void SpawnEnemies()
    {
        if (dialogueManager.dialogueOver == true && numOfEnemiesSpawned < waveControllerScript.numberOfEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                // Choose a random spawn point from the array
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Transform spawnPoint = spawnPoints[randomIndex];

                if (waveControllerScript.enemyType == 1)
                {
                    // Instantiate the enemy prefab at the chosen spawn point
                    Instantiate(Enemy, spawnPoint.position, spawnPoint.rotation);
                }
                else if (waveControllerScript.enemyType == 2)
                {
                    // Instantiate the enemy prefab at the chosen spawn point
                    Instantiate(Enemy2, spawnPoint.position, spawnPoint.rotation);
                }
                else if (waveControllerScript.enemyType == 3)
                {
                    // Instantiate the enemy prefab at the chosen spawn point
                    Instantiate(Enemy3, spawnPoint.position, spawnPoint.rotation);
                }



                //count every time an enemy is spawned
                numOfEnemiesSpawned++;
            }
        }
    }

    public int ReturnWaveNum()
    {
        return waveNum;
    }
}
