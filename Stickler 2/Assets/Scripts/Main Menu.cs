using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    Button playButton;
    Button quitButton;

    //highscore stuff
    public Button highscoreButton;
    public Button highscoreBackButton;

    public GameObject highscoreBackground;

   

    

    // Start is called before the first frame update
    void Start()
    {
        playButton = GameObject.Find("Play Button").GetComponent<Button>();
        quitButton = GameObject.Find("Quit Button").GetComponent<Button>();
        playButton.onClick.AddListener(OnPlayButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
        highscoreButton.onClick.AddListener(OnHighscoreButtonClick);
        highscoreBackButton.onClick.AddListener(OnHighscoreBackButtonClick);
    }

    void OnPlayButtonClick()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("Waves");
    }

    void OnQuitButtonClick()
    {
        Application.Quit();
    }

    void OnHighscoreButtonClick()
    {
        highscoreBackground.gameObject.SetActive(true);
    }

    void OnHighscoreBackButtonClick()
    {
        highscoreBackground.gameObject.SetActive(false);
    }

    

    

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
