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
        List<NodeData> allNodes = new List<NodeData>();
        foreach (BaseNode node in graphView.nodes.ToList())
        {
            NodeData nodeData = FromBaseNode(node);
            allNodes.Add(nodeData);
        }
        dialogueSO.nodesData = allNodes;
        AssetDatabase.CreateAsset(dialogueSO, $"Assets/DialogueSystem/Runtime/{filename}.asset");
        AssetDatabase.SaveAssets();
    }


    public void Load(DialogueSO dialogue)
    {
        graphView.DeleteElements(graphView.graphElements.ToList());
        foreach (var nodeData in dialogue.nodesData) {
            BaseNode newNode = CreateNode(nodeData);
            newNode.RefreshExpandedState();
        }
        foreach (BaseNode node in graphView.nodes) { graphView.ConnectNodes(node); }
    }

    private BaseNode CreateNode(NodeData nodeData)
    {
        BaseNode newNode = ToBaseNode(nodeData);
        newNode.SetPosition(new Rect(newNode.graphPosition, Vector2.zero));
        newNode.Draw();
        newNode.GraphView(graphView);
        graphView.AddElement(newNode);
        return newNode;
    }
    
    // switch data type for storage and instansing 
    private static NodeData FromBaseNode(BaseNode baseNode)
    {
        NodeData nodeData = new NodeData();
        nodeData.nodeType = baseNode.nodeType;
        nodeData.customNodeName = baseNode.customNodeName;
        nodeData.choices = baseNode.choices;
        nodeData.text = baseNode.text;
        nodeData.GUID = baseNode.GUID;
        nodeData.graphPosition = baseNode.graphPosition;
        return nodeData;
    }

    private BaseNode ToBaseNode(NodeData nodeData)
    {
        BaseNode newNode;
        switch (nodeData.nodeType)
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
        newNode.nodeType = nodeData.nodeType;
        newNode.customNodeName = nodeData.customNodeName;
        newNode.choices = nodeData.choices;
        newNode.text = nodeData.text;
        newNode.GUID = nodeData.GUID;
        newNode.graphPosition = nodeData.graphPosition;
        return newNode;
    }

}