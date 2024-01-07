using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{   
    private DialogueSO dialogueSO;
    List<NodeDataSO> nodeData;
    private NodeDataSO lastDialogueNode;
    private NodeDataSO currentNode;
    public TMP_Text dialogueText;
    public Button choiceButtonPrefab;
    public Transform choicePanel;
    private bool hasPlayerInteracted;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartDialogue(DialogueSO dialogueSO,bool playerInteracted = false ) //this might reset every time so might have to set a var in dialogueSO to keep the position
    {
        hasPlayerInteracted = playerInteracted;
        nodeData = dialogueSO.nodesData;
        if (currentNode == null) //if this is first interaction 
        {
            foreach (NodeDataSO node in nodeData) //search nodes
            {
                if (node.nodeTypeData == "start") //for start node
                {
                    currentNode = node; // and assign as current node
                }
            }
        }
        RunCurrentNode();
    }

    private void RunCurrentNode()
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
                if(flagData.isFlagEnabled.ToString() == currentNode.triggerValueData)
                {
                    GetNextNode();
                }
                else
                {
                    currentNode = lastDialogueNode;
                }
            }
        }
        
    }

    private void RunBasicDialogueNode()
    {
        if(hasPlayerInteracted)
        {
            dialogueText.text = currentNode.textData;

            if (Input.GetKeyUp(KeyCode.Space))
            {
                lastDialogueNode = currentNode;
                GetNextNode();
            }
        }
        
    }

    private void RunEndNode()
    {
        Debug.Log("End of node reached.");
    }

    private void RunDialogueNode()
    {
        if (hasPlayerInteracted)
        {
            dialogueText.text = currentNode.textData;
            foreach (ChoiceDataSO choices in currentNode.choicesData)
            {
                Button choiceButton = Instantiate(choiceButtonPrefab, choicePanel);
                choiceButton.GetComponentInChildren<TMP_Text>().text = choices.choiceData;
                choiceButton.onClick.AddListener(delegate { GetNextNode(choices.indexData); });
                lastDialogueNode = currentNode;
            }
        }
        
    }

    private void GetNextNode(int choice = 0)
    {
        string nextNode = currentNode.choicesData[choice].edgeDataData.targetNodeGuidData;
        foreach(NodeDataSO node in nodeData)
        {
            if(node.GUIDData == nextNode)
            {
                currentNode = node;
                RunCurrentNode();
            }
        }
    }
}
