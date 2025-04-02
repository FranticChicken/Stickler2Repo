using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSpider : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueBox;

    public void Interact()
    {       
            dialogueBox.gameObject.SetActive(true);
    }

   
}
