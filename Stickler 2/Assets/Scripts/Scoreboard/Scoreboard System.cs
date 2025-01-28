using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardSystem : MonoBehaviour
{
    List<HighscoreElement> highscoreList = new List<HighscoreElement>();
    [SerializeField] int maxCount = 10;
    [SerializeField] string filename;

    public delegate void OnHighscoreListChanged(List<HighscoreElement> list);
    public static event OnHighscoreListChanged HighscoreListChanged;

    public MainMenu mainMenuScript; 

    private void Start()
    {
        LoadHighscores();
    }

    private void LoadHighscores()
    {
        highscoreList = FileHandler.ReadListFromJSON<HighscoreElement>(filename);

        //keep only 10 entries in the list
        while(highscoreList.Count > maxCount)
        {
            highscoreList.RemoveAt(maxCount);
        }

        if(HighscoreListChanged != null)
        {
            HighscoreListChanged.Invoke(highscoreList);
        }

    }

    private void SaveHighscore()
    {
        FileHandler.SaveToJSON<HighscoreElement>(highscoreList, filename);
    }

    public void AddHighscoreIfPossible (HighscoreElement element)
    {
        for(int i = 0; i < maxCount; i++)
        {
            if(i >= highscoreList.Count || element.waves > highscoreList[i].waves)
            {
                //add new highscore
                highscoreList.Insert(i, element);

                while(highscoreList.Count > maxCount)
                {
                    highscoreList.RemoveAt(maxCount);
                }

                SaveHighscore();

                if (HighscoreListChanged != null)
                {
                    HighscoreListChanged.Invoke(highscoreList);
                }


                break;
            }
        }
    }

    private void Update()
    {
        //mainMenuScript.UpdateUI(highscoreList);
    }
}
