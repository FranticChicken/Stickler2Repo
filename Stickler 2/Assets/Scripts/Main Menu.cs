using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Button playButton;
    Button quitButton; 

    // Start is called before the first frame update
    void Start()
    {
        playButton = GameObject.Find("Play Button").GetComponent<Button>();
        quitButton = GameObject.Find("Quit Button").GetComponent<Button>();
        playButton.onClick.AddListener(OnPlayButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    void OnPlayButtonClick()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("Waves");
    }

    void OnQuitButtonClick()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
