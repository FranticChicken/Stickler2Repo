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
    public Button scoreboardButton;
    public GameObject scoreboardBackground;
    public Button scoreboardBackButton;

    private GameObject crosshair; 

    // Start is called before the first frame update
    void Start()
    {
        //restartButton = GameObject.Find("Restart Button").GetComponent<Button>();
        //mainMenuButton = GameObject.Find("Main Menu Button").GetComponent<Button>();
        //gameOverScreen = GameObject.Find("Game Over Screen");
        restartButton.onClick.AddListener(OnRestartButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        scoreboardButton.onClick.AddListener(OnScoreboardButtonClick);
        scoreboardBackButton.onClick.AddListener(OnScoreboardBackButtonClick);
        
        gameOverScreen.gameObject.SetActive(false);
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
    }

    void OnRestartButtonClick()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.gameObject.SetActive(true);
        playerDead = false;
        SceneManager.LoadScene("Waves");
    }

    void OnMainMenuButtonClick()
    {
        Time.timeScale = 1;
        playerDead = false;
        SceneManager.LoadScene("Main Menu");

    }

    void OnScoreboardButtonClick()
    {
        scoreboardBackground.SetActive(true);
        Debug.Log("scoreboard button clicked");
    }
    void OnScoreboardBackButtonClick()
    {
        scoreboardBackground.SetActive(false);
    }

    public void GameOver()
    {
        playerDead = true;

        if (playerDead == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            gameOverScreen.gameObject.SetActive(true);
            crosshair.gameObject.SetActive(false);
            
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (playerDead)
        {
            Time.timeScale = 0;
        }
       
    }
}
