using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WavesController : MonoBehaviour
{
    //wave controller stats
    [HideInInspector]
    int waveNumber;
    [HideInInspector]
    public float spawnDelay;
    [HideInInspector]
    public int numberOfEnemies;
    [HideInInspector]
    public int spidersKilled = 0;
    [HideInInspector]
    public int enemyType;

    //wave number UI
    public TextMeshProUGUI waveNumText;

    //Enemy Spawner Script
    public EnemySpawner enemySpawnerScript;

    //Dialogue Manager Script
    public DialogueManager dialogueManager;


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
            enemyType = 1;
        }
        
        if(waveNumber >= 5 && waveNumber <= 9)
        {
            spawnDelay = 3f;
            numberOfEnemies = 6;
            enemyType = 2; 
        }

        if(waveNumber >= 10 && waveNumber <= 14)
        {
            spawnDelay = 2f;
            numberOfEnemies = 9;
            enemyType = 2;
        }

        if(waveNumber >= 15 && waveNumber <= 19)
        {
            spawnDelay = 1f;
            numberOfEnemies = 12;
            enemyType = 3;
        }

        if(waveNumber >= 20)
        {
            spawnDelay = 1f;
            numberOfEnemies = 15;
            enemyType = 3;
        }

        waveNumText.text = "Wave " + waveNumber.ToString();

        if(spidersKilled == numberOfEnemies)
        {
            waveNumber++;
            spidersKilled = 0;
            Debug.Log("wave num should increase");
        }

        if(dialogueManager.dialogueOver == false)
        {
            waveNumText.gameObject.SetActive(false);
        }
        else
        {
            waveNumText.gameObject.SetActive(true);
        }

    }
}
