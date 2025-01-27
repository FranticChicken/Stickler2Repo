using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    
    private List<Transform> highscoreEntrytTransformList;

    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);

        AddHighscoreEntry(1, "Test");

        //get player score from json
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //sorting highscores from highest to lowest
        
        for(int i=0; i<highscores.highscoreEntryList.Count; i++)
        {
            for(int j=i+1; j < highscores.highscoreEntryList.Count; j++)
            {
                if(highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    //Swap score positions if one has a higher score than the other
                    HighScoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }
        

        highscoreEntrytTransformList = new List<Transform>();
        foreach(HighScoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreatingHighScoreEntryTransform(highscoreEntry, entryContainer, highscoreEntrytTransformList);
        }

        Debug.Log(PlayerPrefs.GetString("highscoreTable"));
        
    }

    private void CreatingHighScoreEntryTransform(HighScoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 20f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank.ToString() + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;

        }

        entryTransform.Find("POS 1").GetComponent<TextMeshProUGUI>().text = rankString;
        int score = highscoreEntry.score;
        entryTransform.Find("SCORE 1").GetComponent<TextMeshProUGUI>().text = score.ToString();
        string name = highscoreEntry.name;
        entryTransform.Find("NAME 1").GetComponent<TextMeshProUGUI>().text = name.ToString();

        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name)
    {
        //create HighscoreEntry
        HighScoreEntry highscoreEntry = new HighScoreEntry { score = score, name = name };

        //load saved highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        //save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighScoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighScoreEntry
    {
        public int score;
        public string name;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
