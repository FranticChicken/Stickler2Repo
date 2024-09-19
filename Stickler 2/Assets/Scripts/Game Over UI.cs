using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    Button restartButton;
    Button mainMenuButton;
    bool playerDead;
    GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        restartButton = GameObject.Find("Restart Button").GetComponent<Button>();
        mainMenuButton = GameObject.Find("Main Menu Button").GetComponent<Button>();
        gameOverScreen = GameObject.Find("Game Over Screen");
        restartButton.onClick.AddListener(OnRestartButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        //playerDead = true;
        gameOverScreen.gameObject.SetActive(false);
    }

    void OnRestartButtonClick()
    {
        SceneManager.LoadScene("Waves");
    }

    void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GameOver()
    {
        playerDead = true; 
    }


    // Update is called once per frame
    void Update()
    {
        if (playerDead)
        {
            gameOverScreen.gameObject.SetActive(true);
        }
    }
}
