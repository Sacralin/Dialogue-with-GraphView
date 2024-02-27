using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public DialogueInput dialogueInput;
    private DialogueSO dialogueSO;
    private NodeDataSO currentNode;
    public TMP_Text dialogueText;
    public Button choiceButtonPrefab;
    public Transform choicePanel;
    public GameObject dialoguePanelObject;
    public GameObject choicePanelObject;
    private bool hasDialogueStarted;
    private bool inputChanged;
    private bool areButtonsAdded;
    private bool keyReleased;
    private InputSystemManager inputSystemManager;


    // Start is called before the first frame update
    void Start()
    {
        inputSystemManager = FindAnyObjectByType<InputSystemManager>();
        NodeAndFlagAssetStateManager assetStateManager = new NodeAndFlagAssetStateManager();
        assetStateManager.ResetAllFlagAssets();
        assetStateManager.ResetAllNodeAssets();
        dialogueInput = new DialogueInput();
        dialogueInput.DialogueControls.Enable();
        dialoguePanelObject.SetActive(false);
        choicePanelObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inputChanged)
        {
            if (hasDialogueStarted)
            {
                inputSystemManager.currentState = InputSystemManager.GameState.Dialogue;
                inputChanged = false;
            }
            else if (hasDialogueStarted == false && dialogueInput.DialogueControls.NextDialogue.ReadValue<float>() == 0f)
            {
                inputSystemManager.currentState = InputSystemManager.GameState.Character;
                inputChanged = false;
            }
        }
        
        RunCurrentNode();

        
    }

    


    public void StartDialogue(DialogueSO dialogue,bool dialogueStarted = false ) //this might reset every time so might have to set a var in dialogueSO to keep the position
    {
        hasDialogueStarted = dialogueStarted; //check if the player has started a dialogue (determines if we run dialogue nodes or not)
        if (hasDialogueStarted) { inputChanged = true; }
        if (dialogue != dialogueSO) // check if a new dialogue has been started
        {
            currentNode = null;

            if (dialogue.currentNode.nodeTypeData != null)
            {
                currentNode = dialogue.currentNode;
            }
        }
        dialogueSO = dialogue;
        if (currentNode == null) //if this is first interaction 
        {
            foreach (NodeDataSO node in dialogueSO.nodesData) //search nodes
            {
                if (node.nodeTypeData == "Start") //for start node
                {
                    currentNode = node; // and assign as current node
                }
            }
        }
        
    }

    private void RunCurrentNode()
    {
        if(currentNode != null)
        {
            switch (currentNode.nodeTypeData)
            {
                case "Start":
                    GetNextNode();
                    break;
                case "Dialogue":
                    RunDialogueNode();
                    break;
                case "End":
                    RunEndNode();
                    break;
                case "BasicDialogue":
                    RunBasicDialogueNode();
                    break;
                case "Flag":
                    RunFlagNode();
                    break;
                case "Event":
                    RunEventNode();
                    break;
                default:
                    Debug.Log("Node Type Not Found!");
                    break;
            }
        }
        
    }

    private void RunEventNode()
    {
        if(currentNode.eventTypeData == "Toggle Flag Value") //if event is toggle flag
        {
            FlagNodeTools flagNodeTools = new FlagNodeTools();
            List<FlagSO> allFlagAssets = flagNodeTools.GetAllFlagAssets(); //get all flags
            foreach(FlagSO flagSO in allFlagAssets) 
            {
                if(flagSO.name == currentNode.flagObjectData) //find target flag object
                {
                    FlagSO targetFlagSO = flagSO;
                    foreach(FlagData flagData in flagSO.flagDatas) 
                    {
                        if(flagData.flagName == currentNode.triggerFlagData) 
                        {
                            if(currentNode.triggerValueData == "True")
                            {
                                flagData.isFlagEnabled = true;
                            }
                            else
                            {
                                flagData.isFlagEnabled = false;
                            }
                            GetNextNode();

                        }
                    }
                }
            }
        }
    }

    private void RunFlagNode()
    {
        FlagNodeTools flagNodeTools = new FlagNodeTools();
        FlagSO flagAsset = flagNodeTools.GetFlagSO(flagNodeTools.GetAllFlagAssets(), currentNode.flagObjectData);
        foreach(FlagData flagData in flagAsset.flagDatas)
        {
            if(flagData.flagName == currentNode.triggerFlagData)
            {
                if (flagData.isFlagEnabled)
                {
                    //get next node port/choices 0
                    GetNextNode(0);
                }
                else
                {
                    //get next node port/choices 1
                    GetNextNode(1);
                }
            }
        }
    }

    private void RunBasicDialogueNode()
    {
        
        if(hasDialogueStarted)
        {
            dialoguePanelObject.SetActive(true);
            dialogueText.text = currentNode.textData;

            float interact = dialogueInput.DialogueControls.NextDialogue.ReadValue<float>();
            if (interact > 0 && keyReleased)
            {

                keyReleased = false;
                dialogueText.text = "";
                dialoguePanelObject.SetActive(false);
                GetNextNode();
            }
            else
            {
                if (interact == 0)
                {
                    keyReleased = true;
                }

            }
            
            
        }
        
    }

    private void RunEndNode()
    {
        
        hasDialogueStarted = false;
        inputChanged = true;
        if (currentNode.eventTypeData == "Return To Node")
        {
            foreach (NodeDataSO node in dialogueSO.nodesData) 
            {
                if (currentNode.triggerFlagData == node.GUIDData) //end node triggerFlagData contains a GUID to return to
                {
                    currentNode = node; 
                }
            }
        }
    }

    private void RunDialogueNode()
    {
        if (hasDialogueStarted)
        {
            //DiableAllOtherControls();
            dialogueText.text = currentNode.textData;
            dialoguePanelObject.SetActive(true);
            choicePanelObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if(areButtonsAdded == false)
            {
                foreach (ChoiceDataSO choices in currentNode.choicesData)
                {
                    Button choiceButton = Instantiate(choiceButtonPrefab, choicePanel);
                    choiceButton.GetComponentInChildren<TMP_Text>().text = choices.choiceData;
                    choiceButton.onClick.AddListener(delegate { 
                        ClearButtons();
                        GetNextNode(choices.indexData); 

                    });
                }
                areButtonsAdded = true;
            }
            
        }
        
    }

    private void ClearButtons()
    {
        foreach (Transform child in choicePanel)
        {
            Button button = child.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            Destroy(child.gameObject);
        }
        
        dialogueText.text = "";
        dialoguePanelObject.SetActive(false);
        choicePanelObject.SetActive(false);
        areButtonsAdded = false;
    }

    private void GetNextNode(int choice = 0)
    {
        string nextNode = currentNode.choicesData[choice].edgeDataData.targetNodeGuidData;
        foreach(NodeDataSO node in dialogueSO.nodesData)
        {
            if(node.GUIDData == nextNode)
            {
                currentNode = node;
                dialogueSO.currentNode = node;
                RunCurrentNode();
            }
        }
    }

    
}
