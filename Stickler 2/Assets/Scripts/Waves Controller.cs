using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;




public class WavesController : MonoBehaviour
{
    //wave controller stats
    [HideInInspector]
    public int waveNumber;
    [HideInInspector]
    public float spawnDelay;
    [HideInInspector]
    public int numberOfEnemies;
    [HideInInspector]
    public int spidersKilled = 0;
    [HideInInspector]
    public int enemyType;

    public List<WaveData> test2;

    //wave number UI
    public TextMeshProUGUI waveNumText;
    public TextMeshProUGUI waveNumText2;

    //Enemy Spawner Script
    public EnemySpawner enemySpawnerScript;

    //Dialogue Manager Script
    public DialogueManager dialogueManager;

    //Baby Spiders
    [HideInInspector]
    public int numOfBabySpiders;
    [HideInInspector]
    public bool allEnemiesKilled;

    


    // Start is called before the first frame update
    void Start()
    {
        waveNumber = 1;
        numOfBabySpiders = 0;
        allEnemiesKilled = false;
        
    }

    // Update is called once per frame
    void Update()
    { 
        if (waveNumber >= 0 && waveNumber <= 2)
        {
            spawnDelay = 5f;
            numberOfEnemies = 3;
            enemyType = 1;
        }
        
        if(waveNumber == 3)
        {
            spawnDelay = 2f;
            numberOfEnemies = 3;
            enemyType = 2; 
        }

        if(waveNumber >= 4 && waveNumber <= 5)
        {
            spawnDelay = 0.5f;
            numberOfEnemies = 3;
            enemyType = 3;
        }

        if(waveNumber >= 6 && waveNumber <= 8)
        {
            spawnDelay = 0.5f;
            numberOfEnemies = 6;
            enemyType = 12;
        }

        if(waveNumber >= 9 && waveNumber <= 11)
        {
            spawnDelay = 0.5f;
            numberOfEnemies = 6;
            enemyType = 123;
        }

        if(waveNumber >= 12)
        {
            spawnDelay = 0.5f;
            numberOfEnemies = 12;
            enemyType = 123;
        }

        waveNumText.text = "Wave " + waveNumber.ToString();
        waveNumText2.text = "Wave " + waveNumber.ToString();

        if(spidersKilled == numberOfEnemies + numOfBabySpiders)
        {
            waveNumber++;
            spidersKilled = 0;

            //Debug.Log("wave num should increase");
            StartCoroutine(NewWaveEffect());
        }

        IEnumerator NewWaveEffect()
        {
            waveNumText.gameObject.SetActive(false);
            waveNumText2.gameObject.SetActive(true);
            

            yield return new WaitForSeconds(3);

            waveNumText.gameObject.SetActive(true);
            waveNumText2.gameObject.SetActive(false);
            
        }

        if (dialogueManager.dialogueOver == false)
        {
            waveNumText.gameObject.SetActive(false);

        }
        else
        {
            waveNumText.gameObject.SetActive(true);
            
        }

        
    }

    public bool WaveFinished()
    {
        //used to replenish health & ammo in player script
        if (spidersKilled == numberOfEnemies + numOfBabySpiders)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }
}

[System.Serializable]
public struct WaveData
{

    public float test;
}
