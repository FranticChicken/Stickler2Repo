using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardNameInput : MonoBehaviour
{
    public Button submitButton;
    public GameObject leaderboardInputBackground;
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
        leaderboardInputBackground.SetActive(false);
        submitClicked = true;

    }

    // Update is called once per frame
    void Update()
    {
        /*if (wavesControllerScript.waveNumber > JsonUtility.FromJson<Highscores>(PlayerPrefs.GetString("highscoreTable")).highscoreEntryList[9].score)
        {
            canSetNewHighscore = true;
        }
        else
        {
            canSetNewHighscore = false;
        }
        */
        //&& leaderboardScript.canSetNewHighscore == true
        if (gameOverUIScript.playerDead == true && submitClicked == false && leaderboardScript.canSetNewHighscore == true)
        {
            leaderboardInputBackground.SetActive(true);
        }
    }
}
