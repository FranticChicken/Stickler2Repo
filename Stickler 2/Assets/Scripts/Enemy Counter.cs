using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    public TextMeshProUGUI enemyCounterText;
    WavesController wavesControllerScript;
    int enemiesLeft;

    //Dialogue Manager Script
    public DialogueManager dialogueManager;

    public GameOverUI gameOverScript;

    // Start is called before the first frame update
    void Start()
    {
        wavesControllerScript = GameObject.FindGameObjectWithTag("waves").GetComponent<WavesController>();
    }

    // Update is called once per frame
    void Update()
    {
        enemiesLeft = wavesControllerScript.numberOfEnemies - wavesControllerScript.spidersKilled;
        enemyCounterText.text =  "Enemies Left = " + enemiesLeft.ToString();

        if (dialogueManager.dialogueOver == false || gameOverScript.playerDead == true)
        {
            enemyCounterText.gameObject.SetActive(false);
        }
        else
        {
            enemyCounterText.gameObject.SetActive(true);
        }
    }
}
