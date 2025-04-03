using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSpider : MonoBehaviour
{
    [SerializeField] private SpiderDialogueManager dialogueBox;

    public void Interact()
    {
        dialogueBox.dialogueOver = false;
        dialogueBox.dialogueStarted = true;
    }

   
}
