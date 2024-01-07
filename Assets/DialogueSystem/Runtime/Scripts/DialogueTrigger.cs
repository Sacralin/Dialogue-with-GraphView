using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueSO dialogueSO;
    private DialogueManager dialogueManager;

    void Start()
    {
        GameObject dialogueManagerObject = GameObject.Find("DialogueManager");
        if (dialogueManagerObject != null)
        {
            dialogueManager = dialogueManagerObject.GetComponent<DialogueManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.StartDialogue(dialogueSO);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E)) 
            {
                Debug.Log("Player Interacted!");

                dialogueManager.StartDialogue(dialogueSO, true);
            }
            
        }
    }
}
