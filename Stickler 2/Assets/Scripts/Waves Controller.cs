using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WavesController : MonoBehaviour
{
    //wave controller stats
    int waveNumber;
    public float spawnDelay;
    public int numberOfEnemies;
    public int spidersKilled; 

    //wave number UI
    public TextMeshProUGUI waveNumText;

    //Enemy Spawner Script
    public EnemySpawner enemySpawnerScript;


    // Start is called before the first frame update
    void Start()
    {
        waveNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //waveNumber = enemySpawnerScript.ReturnWaveNum();

        if (waveNumber >= 0 && waveNumber <= 4)
        {
            spawnDelay = 5f;
            numberOfEnemies = 3;
        }
        
        if(waveNumber >= 5 && waveNumber <= 9)
        {
            spawnDelay = 4f;
            numberOfEnemies = 6;
        }

        if(waveNumber >= 10 && waveNumber <= 14)
        {
            spawnDelay = 3f;
            numberOfEnemies = 9;
        }

        if(waveNumber >= 15 && waveNumber <= 19)
        {
            spawnDelay = 2f;
            numberOfEnemies = 12;
        }

        if(waveNumber >= 20)
        {
            spawnDelay = 1f;
            numberOfEnemies = 15;
        }

        waveNumText.text = "Wave " + waveNumber.ToString();

        if(spidersKilled == numberOfEnemies)
        {
            waveNumber++;
            spidersKilled = 0;
            Debug.Log("wave num should increase");
        }

    }
}
