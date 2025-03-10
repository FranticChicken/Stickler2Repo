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

    


    // Start is called before the first frame update
    void Start()
    {
        waveNumber = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        //waveNumber = enemySpawnerScript.ReturnWaveNum();

        if (waveNumber >= 0 && waveNumber <= 2)
        {
            spawnDelay = 5f;
            numberOfEnemies = 3;
            enemyType = 1;
        }
        
        if(waveNumber >= 3 && waveNumber <= 5)
        {
            spawnDelay = 2f;
            numberOfEnemies = 6;
            enemyType = 2; 
        }

        if(waveNumber >= 6 && waveNumber <= 8)
        {
            spawnDelay = 0.5f;
            numberOfEnemies = 9;
            enemyType = 2;
        }

        if(waveNumber >= 9 && waveNumber <= 11)
        {
            spawnDelay = 0.5f;
            numberOfEnemies = 12;
            enemyType = 3;
        }

        if(waveNumber >= 12)
        {
            spawnDelay = 0.5f;
            numberOfEnemies = 15;
            enemyType = 3;
        }

        waveNumText.text = "Wave " + waveNumber.ToString();
        waveNumText2.text = "Wave " + waveNumber.ToString();

        if(spidersKilled == numberOfEnemies)
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
}

[System.Serializable]
public struct WaveData
{

    public float test;
}
