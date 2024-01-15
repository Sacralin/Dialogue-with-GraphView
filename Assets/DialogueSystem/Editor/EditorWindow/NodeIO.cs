using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeIO
{
    DialogueGraphView graphView;

    public NodeIO(DialogueGraphView dialogueGraphView)
    {
        graphView = dialogueGraphView;
    }

    public void Save(string filename)
    {
        List<NodeDataSO> allNodes = new List<NodeDataSO>();
        foreach (BaseNode node in graphView.nodes.ToList())
        {
            NodeDataSO nodeData = FromBaseNode(node);
            allNodes.Add(nodeData);
        }
        bool assetFound = false;
        string[] assetList = AssetDatabase.FindAssets("t:DialogueSO"); // find all flag stores in project, returns asset GUIDs
        if (assetList.Length != 0)
        {
            foreach (string asset in assetList)
            {
                string SOpath = AssetDatabase.GUIDToAssetPath(asset); // convert GUID into asset path
                DialogueSO diaSO = AssetDatabase.LoadAssetAtPath<DialogueSO>(SOpath); // load asset from path
                if(diaSO.name == filename)
                {
                    diaSO.nodesData = allNodes;
                    EditorUtility.SetDirty(diaSO);
                    assetFound = true;
                }
            }
        }
        if (!assetFound) 
        {
            DialogueSO dialogueSO = ScriptableObject.CreateInstance<DialogueSO>();
            dialogueSO.nodesData = allNodes;
            AssetDatabase.CreateAsset(dialogueSO, $"Assets/DialogueSystem/Runtime/{filename}.asset");
        }
        AssetDatabase.SaveAssets();
    }


    public void Load(DialogueSO dialogue)
    {
        graphView.DeleteElements(graphView.graphElements.ToList());
        foreach (var nodeDataSO in dialogue.nodesData) {
            BaseNode newNode = CreateNode(nodeDataSO);
            newNode.RefreshExpandedState();
            //Debug.Log(nodeDataSO.graphPositionData + nodeDataSO.nodeTypeData + "Node");
            //BaseNode baseNode = graphView.CreateNode(nodeDataSO.graphPositionData, nodeDataSO.nodeTypeData+"Node");
            //SetBaseNode(baseNode, nodeDataSO);
            //graphView.AddElement(baseNode);




        }
        foreach (BaseNode node in graphView.nodes) 
        {
            foreach (ChoiceData choiceData in node.choices)
            {
                Port targetPort = node.outputContainer.Query<Port>().AtIndex(choiceData.index);
                targetPort.name = choiceData.portName;
            }
            graphView.ConnectNodes(node); 
        }
    }

    private BaseNode CreateNode(NodeDataSO nodeData)
    {
        BaseNode newNode = ToBaseNode(nodeData);
        newNode.SetPosition(new Rect(newNode.graphPosition, Vector2.zero));
        newNode.Draw();
        newNode.GraphView(graphView);
        graphView.AddElement(newNode);
        return newNode;
    }

    // switch data type for storage and instansing 
    private static NodeDataSO FromBaseNode(BaseNode baseNode)
    {
        NodeDataSO nodeData = new NodeDataSO();
        nodeData.nodeTypeData = baseNode.nodeType;
        nodeData.customNodeNameData = baseNode.customNodeName;
        nodeData.textData = baseNode.text;
        nodeData.GUIDData = baseNode.GUID;
        nodeData.graphPositionData = baseNode.graphPosition;
        nodeData.flagObjectData = baseNode.flagObject;
        nodeData.triggerFlagData = baseNode.triggerFlag;
        nodeData.triggerValueData = baseNode.triggerValue;
        nodeData.eventTypeData = baseNode.eventType;
        nodeData.choicesData = new List<ChoiceDataSO>();
        if(baseNode.choices.Count != 0)
        {
            foreach (ChoiceData baseChoice in baseNode.choices)
            {
                ChoiceDataSO choiceDataSO = new ChoiceDataSO();
                choiceDataSO.choiceData = baseChoice.choice;
                choiceDataSO.portNameData = baseChoice.portName;
                choiceDataSO.indexData = baseChoice.index;
                if(baseChoice.edgeData != null)
                {
                    EdgeDataSO edgeDataSO = new EdgeDataSO();
                    edgeDataSO.targetNodeGuidData = baseChoice.edgeData.targetNodeGuid;
                    edgeDataSO.sourceNodeGuidData = baseChoice.edgeData.sourceNodeGuid;
                    choiceDataSO.edgeDataData = edgeDataSO;
                    nodeData.choicesData.Add(choiceDataSO);
                }
                
            }
        }
        
        return nodeData;
    }

    private BaseNode ToBaseNode(NodeDataSO nodeData)
    {
        BaseNode newNode;
        switch (nodeData.nodeTypeData)
        {
            case "Start":
                newNode = new StartNode();
                break;
            case "Dialogue":
                newNode = new DialogueNode();
                break;
            case "End":
                newNode = new EndNode();
                break;
            case "BasicDialogue":
                newNode = new BasicDialogueNode();
                break;
            case "Flag":
                newNode = new FlagNode();
                break;
            case "Event":
                newNode = new EventNode();
                break;
            default:
                newNode = new BaseNode();
                break;
        }
        newNode.nodeType = nodeData.nodeTypeData;
        newNode.customNodeName = nodeData.customNodeNameData;
        newNode.text = nodeData.textData;
        newNode.GUID = nodeData.GUIDData;
        newNode.graphPosition = nodeData.graphPositionData;
        newNode.flagObject = nodeData.flagObjectData;
        newNode.triggerFlag = nodeData.triggerFlagData;
        newNode.triggerValue = nodeData.triggerValueData;
        newNode.eventType = nodeData.eventTypeData;
        newNode.choices = new List<ChoiceData>();
        foreach (ChoiceDataSO choiceDataSO in nodeData.choicesData)
        {
            ChoiceData choice = new ChoiceData();
            choice.choice = choiceDataSO.choiceData;
            choice.portName = choiceDataSO.portNameData;
            choice.index = choiceDataSO.indexData;
            EdgeData edgeData = new EdgeData();
            edgeData.targetNodeGuid = choiceDataSO.edgeDataData.targetNodeGuidData;
            edgeData.sourceNodeGuid = choiceDataSO.edgeDataData.sourceNodeGuidData;
            choice.edgeData = edgeData;
            newNode.choices.Add(choice);


        }
        return newNode;
    }

    private void SetBaseNode(BaseNode newNode, NodeDataSO nodeData)
    {
        newNode.nodeType = nodeData.nodeTypeData;
        newNode.customNodeName = nodeData.customNodeNameData;
        newNode.text = nodeData.textData;
        newNode.GUID = nodeData.GUIDData;
        newNode.graphPosition = nodeData.graphPositionData;
        newNode.flagObject = nodeData.flagObjectData;
        newNode.triggerFlag = nodeData.triggerFlagData;
        newNode.triggerValue = nodeData.triggerValueData;
        newNode.eventType = nodeData.eventTypeData;
        newNode.choices = new List<ChoiceData>();
        foreach (ChoiceDataSO choiceDataSO in nodeData.choicesData)
        {
            ChoiceData choice = new ChoiceData();
            choice.choice = choiceDataSO.choiceData;
            //choice.portName = choiceDataSO.portNameData;
            choice.index = choiceDataSO.indexData;
            EdgeData edgeData = new EdgeData();
            edgeData.targetNodeGuid = choiceDataSO.edgeDataData.targetNodeGuidData;
            edgeData.sourceNodeGuid = choiceDataSO.edgeDataData.sourceNodeGuidData;
            choice.edgeData = edgeData;
            newNode.choices.Add(choice);


        }
    }

}