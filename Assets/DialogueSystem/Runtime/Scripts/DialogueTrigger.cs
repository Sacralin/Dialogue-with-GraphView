using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueSO dialogueSO;
    private DialogueManager dialogueManager;
    private DialogueInput dialogueInput;

    void Start()
    {
        dialogueInput = new DialogueInput();
        dialogueInput.Enable();
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
            float interact = dialogueInput.DialogueControls.Interact.ReadValue<float>();
            if (interact > 0 ) 
            {
                dialogueManager.StartDialogue(dialogueSO, true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.HasExited();

        }
    }
}
