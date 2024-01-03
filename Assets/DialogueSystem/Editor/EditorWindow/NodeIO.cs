using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeIO
{
    DialogueGraphView graphView;

    public NodeIO(DialogueGraphView dialogueGraphView)
    {
        graphView = dialogueGraphView;
    }

    public void Save(string filename)
    {
        DialogueSO dialogueSO = ScriptableObject.CreateInstance<DialogueSO>();

        graphView.ClearOldEdgeData();
        List<NodeDataSO> allNodes = new List<NodeDataSO>();
        foreach (BaseNode node in graphView.nodes.ToList())
        {
            NodeDataSO nodeData = FromBaseNode(node);
            allNodes.Add(nodeData);
        }
        dialogueSO.nodesData = allNodes;
        AssetDatabase.CreateAsset(dialogueSO, $"Assets/DialogueSystem/Runtime/{filename}.asset");
        AssetDatabase.SaveAssets();
    }


    public void Load(DialogueSO dialogue)
    {
        graphView.DeleteElements(graphView.graphElements.ToList());
        foreach (var nodeDataSO in dialogue.nodesData) {
            BaseNode newNode = CreateNode(nodeDataSO);
            newNode.RefreshExpandedState();
        }
        foreach (BaseNode node in graphView.nodes) { graphView.ConnectNodes(node); }
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
        nodeData.choicesData = new List<ChoiceDataSO>();
        foreach(ChoiceData baseChoice in baseNode.choices)
        {
            ChoiceDataSO choiceDataSO = new ChoiceDataSO();
            choiceDataSO.choiceData = baseChoice.choice;
            choiceDataSO.portNameData = baseChoice.portName;
            choiceDataSO.indexData = baseChoice.index;
            EdgeDataSO edgeDataSO = new EdgeDataSO();
            edgeDataSO.targetNodeGuidData = baseChoice.edgeData.targetNodeGuid;
            edgeDataSO.sourceNodeGuidData = baseChoice.edgeData.sourceNodeGuid;
            choiceDataSO.edgeDataData = edgeDataSO;
            nodeData.choicesData.Add(choiceDataSO);
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
            default:
                newNode = new BaseNode();
                break;
        }
        newNode.nodeType = nodeData.nodeTypeData;
        newNode.customNodeName = nodeData.customNodeNameData;
        newNode.text = nodeData.textData;
        newNode.GUID = nodeData.GUIDData;
        newNode.graphPosition = nodeData.graphPositionData;
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

}