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

    public GameObject highscoreUIElementPrefab;
    public Transform elementWrapper;

    List<GameObject> uiElements = new List<GameObject>();

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

    private void OnEnable()
    {
        ScoreboardSystem.HighscoreListChanged += UpdateUI;

    }

    private void OnDisable()
    {
        ScoreboardSystem.HighscoreListChanged -= UpdateUI;
    }

    public void UpdateUI(List<HighscoreElement> list)
    {
        Debug.Log("UPDATE UI 1");
        Debug.Log(list.Count);
        for (int i = 0; i <= list.Count; i++)
        {
            HighscoreElement el = list[i];
            Debug.Log("UPDATE UI 2");

            if (el.waves > 0)
            {
                if(i >= uiElements.Count)
                {
                    //instantiate new entry
                    var inst = Instantiate(highscoreUIElementPrefab, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent(elementWrapper, false);

                    uiElements.Add(inst);
                }

                Debug.Log("UPDATE UI");

                var texts = uiElements[i].GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = el.playerName;
                texts[1].text = el.waves.ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
