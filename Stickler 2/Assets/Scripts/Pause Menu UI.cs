using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public Button resumeButton;
    public Button quitButton;
    public Slider mouseSenseSlider;

    public GameObject pauseMenu;

    public InputActionReference Pause;

    public float mouseSense; 

    bool gamePaused = false;

    bool gameOver = false;

    public GameOverUI gameOverScript;

    // Start is called before the first frame update
    void Start()
    {
        resumeButton.onClick.AddListener(OnResumeButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
        mouseSenseSlider.onValueChanged.AddListener(delegate {SliderValueChanged ();});
        mouseSenseSlider.maxValue = 20f;
        Pause.action.performed += TogglePauseMenu;
        mouseSense = 10f;
    }

    void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (gameOverScript.playerDead == false)
        {
            gamePaused = !gamePaused;
        }
    }

    void OnResumeButtonClick()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gamePaused = !gamePaused;
    }

    void OnQuitButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
    
    void SliderValueChanged()
    {
        mouseSense = mouseSenseSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (!gamePaused && gameOverScript.playerDead == false)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        mouseSenseSlider.value = mouseSense;
    }
}
