using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class Leaderboard : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    //List<HighscoreEntry> highscoreEntryList;
    List<Transform> highscoreEntryTransformList;

    public WavesController wavesControllerScript;
    
    [HideInInspector]
    public bool canSetNewHighscore = false;

    private const string LEADERBOARD_FILE_NAME = "leaderboard.mysavefile";
    private FileStream m_LeaderboardStream;

    struct LeaderboardEntry 
    {
        public string name;
        public int score;
    }

    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();

    private void Awake()
    {
        //Application.runInBackground = true;
        //creating a file named LEADERBOARD_FILE_NAME in the assets folder 
        string leaderboardFilePath = Path.Combine(UnityEngine.Application.dataPath, LEADERBOARD_FILE_NAME);
        Debug.Log(leaderboardFilePath);
        //open file and if there is no file, create one
        m_LeaderboardStream = File.Open(leaderboardFilePath, FileMode.OpenOrCreate);

        //create bytearray of file byte size
        byte[] byteArray = new byte[m_LeaderboardStream.Length];
        //read file and learn how many bytes the text makes up in bytes 
        int bytesRead = m_LeaderboardStream.Read(byteArray, 0, byteArray.Length);
        //turn bytes into string
        string entriesString = Encoding.ASCII.GetString(byteArray);
        //split string into array of strings of each line
        string [] splitLines = entriesString.Split('\n');


        for (int i = 0; i < splitLines.Length; i++)
        {
            //for each line
            string line = splitLines[i];
            //define each quotation mark placement 
            int firstQuoteIndex = line.IndexOf('"', 0);
            int firstQuoteIndex2 = line.IndexOf('"', firstQuoteIndex + 1);
            int firstQuoteIndex3 = line.IndexOf('"', firstQuoteIndex2 + 1);
            int firstQuoteIndex4 = line.IndexOf('"', firstQuoteIndex3 + 1);

            //define where the name is positioned between the quotation marks
            string name = line.Substring(firstQuoteIndex + 1, firstQuoteIndex2 - (firstQuoteIndex + 1));
            //define where the score is positioned between the quotation marks
            string score = line.Substring(firstQuoteIndex3 + 1, firstQuoteIndex4 - (firstQuoteIndex3 + 1));
            
            //take score string and turn into int
            if(int.TryParse(score, out int intScore))
            {
                LeaderboardEntry newEntry = new LeaderboardEntry();
                newEntry.name = name;
                newEntry.score = intScore;

            }

            
            


        }


        //12:38
        entryTemplate.gameObject.SetActive(false);

        /*highscoreEntryList = new List<HighscoreEntry>()
        {
            new HighscoreEntry{score = 0, name = "test1"},
            new HighscoreEntry{score = 1, name = "test2"},
            new HighscoreEntry{score = 2, name = "test3"},
        };
        */

        //AddHighscoreEntry(3, "Emily");

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //sort entry list by score
        
        for(int i =0; i< highscores.highscoreEntryList.Count; i++)
        {
            for(int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if(highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    //swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        //make highscore list max of 10
        if (highscores.highscoreEntryList.Count > 10)
        {
            for (int h = highscores.highscoreEntryList.Count; h > 10; h--)
            {
                highscores.highscoreEntryList.RemoveAt(10);
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach(HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

        /*
        Highscores highscores = new Highscores { highscoreEntryList = highscoreEntryList };
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("highscoreTable"));
        */
    }

    private void OnDestroy()
    {
        //m_LeaderboardStream.Close();
    }

    void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
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
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
               
        }
        
        entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().text = rankString;
        int score = highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();
        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = name;

        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int score, string name)
    {
        //Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        //stop function if lowest score is higher than current score
        if(leaderboardEntries.Count >= 10)
        {
            if(leaderboardEntries[leaderboardEntries.Count - 1].score >= score)
            {
                return;
            }
        }

        //TODO Place new score in the correct position if applicable
        //REPLACE THIS IDIOT
        LeaderboardEntry entry = new LeaderboardEntry();
        entry.score = score;
        entry.name = name;
        leaderboardEntries.Add(entry);
        //.Insert to put in a certain spot 

        //Write to file
        m_LeaderboardStream.Position = 0;
        m_LeaderboardStream.SetLength(0);

        //setting up how the stream is formated text wise
        string fullList = "";
        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            string fixedName = leaderboardEntries[i].name.Replace('\"', '\'');
            fullList += "\"" + fixedName + "\"" + " \"" + leaderboardEntries[i].score + "\"" + "\n";
        }

        //convert entry into byte array 
        byte[] convertedData = Encoding.ASCII.GetBytes(fullList);
        m_LeaderboardStream.Write(convertedData, 0, convertedData.Length);
        m_LeaderboardStream.Flush();





        return;
        //Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        //sort entry list by score

        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    //swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        //make highscore list max of 10
        if (highscores.highscoreEntryList.Count > 10)
        {
            for (int h = highscores.highscoreEntryList.Count; h > 10; h--)
            {
                highscores.highscoreEntryList.RemoveAt(10);
            }
        }

        //Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    public class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    //reps a single highscore entry
    [System.Serializable]
    public class HighscoreEntry
    {
        public int score;
        public string name;
    }

    private void Update()
    {
        return;

        //canSetNewHighscore = true;

        //Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //sort entry list by score

        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    //swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        //if wave num is more than lowest score in leaderboard, set canSetNewHighScore to true
        
        if (wavesControllerScript.waveNumber > JsonUtility.FromJson<Highscores>(PlayerPrefs.GetString("highscoreTable")).highscoreEntryList[9].score)
        {
            canSetNewHighscore = true;
        }
        else
        {
            canSetNewHighscore = false;
        }

        //Debug.Log(canSetNewHighscore);
        
    }
}
