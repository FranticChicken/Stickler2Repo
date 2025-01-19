using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public Image dialogueBackground;

    public string[] sentences;
    public float textSpeed;

    private int index;

    [HideInInspector]
    public bool dialogueOver;

    
    // Start is called before the first frame update
    void Start()
    {
        
        dialogueOver = false;
        dialogueText.text = string.Empty;
        StartDialogue();
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());

    }

    IEnumerator TypeLine()
    {
        // type each character 1 by 1
        foreach(char c in sentences[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(index < sentences.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueBackground.gameObject.SetActive(false);
            dialogueOver = true;
        }
    }

    //make text appear slowly 
    //make it so you can click to make text appear fully 
    //make it so there's a dialogue skip button 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(dialogueText.text == sentences[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = sentences[index];
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            dialogueBackground.gameObject.SetActive(false);
            
            dialogueOver = true;
        }

        
    }
}
