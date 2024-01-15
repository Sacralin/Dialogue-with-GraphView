using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //private InputActionAsset[] playerInputs;
    //private bool[] isPlayerInputEnabled;
    //public List<string> mapsToDisable = new List<string>();
    private DialogueInput dialogueInput;
    private DialogueSO dialogueSO;
    private NodeDataSO lastDialogueNode;
    private NodeDataSO currentNode;
    public TMP_Text dialogueText;
    public Button choiceButtonPrefab;
    public Transform choicePanel;
    public GameObject dialoguePanelObject;
    public GameObject choicePanelObject;
    private bool hasPlayerInteracted;
    private bool areButtonsAdded;


    // Start is called before the first frame update
    void Start()
    {
        dialogueInput = new DialogueInput();
        dialogueInput.DialogueControls.Enable();
        dialoguePanelObject.SetActive(false);
        choicePanelObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentNode != null)
        {
            Debug.Log(currentNode.nodeTypeData);
        }
        RunCurrentNode();
        
        
    }

    public void HasExited()
    {
        //dialogueSO = null;
    }


    public void StartDialogue(DialogueSO dialogue,bool playerInteracted = false ) //this might reset every time so might have to set a var in dialogueSO to keep the position
    {
        hasPlayerInteracted = playerInteracted;
        //dialogueSO = ScriptableObject.CreateInstance<DialogueSO>();
        if(dialogue != dialogueSO)
        {
            currentNode = null;
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
                            EditorUtility.SetDirty(targetFlagSO); 
                            AssetDatabase.SaveAssets();
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
                Debug.Log(flagData.isFlagEnabled);
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
        if(hasPlayerInteracted)
        {
            //DiableAllOtherControls();
            dialoguePanelObject.SetActive(true);
            dialogueText.text = currentNode.textData;

            if (dialogueInput.DialogueControls.Interact.triggered)
            {
                lastDialogueNode = currentNode;
                dialogueText.text = "";
                dialoguePanelObject.SetActive(false);
                GetNextNode();
            }
        }
        
    }

    private void RunEndNode()
    {
        hasPlayerInteracted = false;
        if (currentNode.eventTypeData == "Return To Node")
        {
            foreach (NodeDataSO node in dialogueSO.nodesData) 
            {
                if (currentNode.triggerFlagData == node.triggerFlagData) 
                {
                    currentNode = node; 
                }
            }
        }
    }

    private void RunDialogueNode()
    {
        if (hasPlayerInteracted)
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
                        Debug.Log("Button Clicked");
                        ClearButtons();
                        GetNextNode(choices.indexData); 

                    });
                    lastDialogueNode = currentNode;
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
        Debug.Log(choice);
        string nextNode = currentNode.choicesData[choice].edgeDataData.targetNodeGuidData;
        foreach(NodeDataSO node in dialogueSO.nodesData)
        {
            if(node.GUIDData == nextNode)
            {
                currentNode = node;
                RunCurrentNode();
            }
        }
    }

    //private void DiableAllOtherControls()
    //{
    //    playerInputs = FindObjectsOfType<InputActionAsset>();
    //    for (int i = 0; i < playerInputs.Length; i++)
    //    {
    //        if (playerInputs[i].name != "DialogueInput")
    //        {
    //            playerInputs[i].Disable();
    //        }
    //    }
    //}

    //private void EnableAllOtherControls()
    //{
    //    for (int i = 0;i < playerInputs.Length; i++)
    //    {
    //        if (playerInputs[i].name != "DialogueInput")
    //        {
    //            playerInputs[i].Enable();
    //            Debug.Log($"Enabled {playerInputs[i].name}");
    //        }
    //    }
    //}
}
