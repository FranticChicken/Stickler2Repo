using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;

    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);

        float templateHeight = 20f;
        for (int i = 0; i < 10; i++)
        {
            //copies entryTemplate and puts it under the parent entryContainer
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);

            int rank = i + 1;
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
            int score = 5;
            entryTransform.Find("SCORE 1").GetComponent<TextMeshProUGUI>().text = score.ToString();
            string name = "EMI";
            entryTransform.Find("NAME 1").GetComponent<TextMeshProUGUI>().text = name.ToString();
        }
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
