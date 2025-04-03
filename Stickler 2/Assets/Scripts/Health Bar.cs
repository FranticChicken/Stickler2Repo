using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;

    public PauseMenuUI pauseMenuScript;
    public GameOverUI gameOverScript;
    public DialogueManager dialogueManagerScript;
    public SpiderDialogueManager spiderDManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }


    // Update is called once per frame
    void Update()
    {
        //if game is paused, player is dead, or dialogue is playing, don't show the health bar
        if (dialogueManagerScript.dialogueOver == false || gameOverScript.playerDead == true || pauseMenuScript.gamePaused == true || spiderDManager.dialogueOver == false)
        {
            healthBar.gameObject.SetActive(false);
        }
        else
        {
            healthBar.gameObject.SetActive(true);
        }


    }
}
