using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public Button restartButton;
    public Button mainMenuButton;
    public bool playerDead = false;
    public GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        //restartButton = GameObject.Find("Restart Button").GetComponent<Button>();
        //mainMenuButton = GameObject.Find("Main Menu Button").GetComponent<Button>();
        //gameOverScreen = GameObject.Find("Game Over Screen");
        restartButton.onClick.AddListener(OnRestartButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        
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
        Debug.Log("player is dead");
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerDead);
        if(playerDead == true)
        {
            gameOverScreen.gameObject.SetActive(true);
            Debug.Log("show game over screen");
        }
    }
}
