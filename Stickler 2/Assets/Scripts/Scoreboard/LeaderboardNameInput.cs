using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardNameInput : MonoBehaviour
{
    public Button submitButton;
    public GameObject leaderboardBackground;
    public Leaderboard leaderboardScript;
    string playerName;
    public WavesController wavesControllerScript;
    public TMP_InputField inputField;
    public GameOverUI gameOverUIScript;
    bool submitClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        submitButton.onClick.AddListener(OnSubmitButtonClick);
        
    }

    void OnSubmitButtonClick()
    {
        Debug.Log("submit button clicked");
        playerName = inputField.text;
        Debug.Log(playerName);
        //scoreboardSystemScript.AddHighscoreIfPossible(new HighscoreElement(playerName, wavesControllerScript.waveNumber));
        leaderboardScript.AddHighscoreEntry(wavesControllerScript.waveNumber, playerName);
        leaderboardBackground.SetActive(false);
        submitClicked = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(gameOverUIScript.playerDead == true && submitClicked == false)
        {
            leaderboardBackground.SetActive(true);
        }
    }
}
