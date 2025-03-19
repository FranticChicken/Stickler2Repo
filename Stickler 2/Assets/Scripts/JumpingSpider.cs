using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSpider : MonoBehaviour
{
    private Collider interactBox;
    private DialogueManager dialogueBox;
    private bool canInteract;

    public void Interact()
    {
        if (canInteract)
        {
            dialogueBox.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canInteract = false;
        }
    }
}
