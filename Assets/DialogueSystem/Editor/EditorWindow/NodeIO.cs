using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.MemoryProfiler;
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
        DialogueSO dialogueSO = ScriptableObject.CreateInstance<DialogueSO>();

        graphView.ClearOldEdgeData();
        dialogueSO.nodesData = SaveNodes().ToList();
        dialogueSO.edgesData = SaveEdges().ToList(); //depreciated

        
        

        AssetDatabase.CreateAsset(dialogueSO, $"Assets/DialogueSystem/Runtime/{filename}.asset");
        AssetDatabase.SaveAssets();
    }



    private List<NodeData> SaveNodes()
    {
        List<NodeData> allNodes = new List<NodeData>();
        foreach (var node in graphView.nodes.ToList())
        {
            LinkEdgesToChoices((BaseNode)node); //deprciated
            NodeData nodeData = FromBaseNode((BaseNode)node);
            allNodes.Add(nodeData);
        }
        return allNodes;
    }

    //depreciated as edge data now saves ongraphchange?
    
    private void LinkEdgesToChoices(BaseNode node) //Adds Edgedata to choices array to centeralise the information
    {
        foreach (ChoiceData choiceData in node.choices)
        {
            Port port = (Port)node.outputContainer.ElementAt(choiceData.index);
            if (port.connected)
            {
                Edge edge = port.connections.First();
                BaseNode connectedNode = (BaseNode)edge.input.node;
                EdgeData edgeData = new EdgeData();
                edgeData.targetNodeGuid = connectedNode.GUID;
                edgeData.sourceNodeGuid = node.GUID;
                choiceData.edgeData = edgeData;
            }
            
        }
    }
    
    //depreciated
    private List<EdgeData> SaveEdges()
    {
        List<EdgeData> allEdges = new List<EdgeData>();
        Edge[] connectedEdges = graphView.edges.Where(edge => edge.input.node != null).ToArray();
        for (int i = 0; i < connectedEdges.Count(); i++)
        {
            BaseNode outputNode = (BaseNode)connectedEdges[i].output.node;
            BaseNode inputNode = (BaseNode)connectedEdges[i].input.node;

            allEdges.Add(new EdgeData
            {
                sourceNodeGuid = outputNode.GUID,
                targetNodeGuid = inputNode.GUID,
            });
        }
        return allEdges;
    }
    

    public void Load(DialogueSO dialogue)
    {
        graphView.DeleteElements(graphView.graphElements.ToList());
        Dictionary<string, BaseNode> createdNodes = new Dictionary<string, BaseNode>();
        LoadNodeData(dialogue, createdNodes);
        LoadEdgeData(dialogue, createdNodes); //depreciated

        foreach (BaseNode node in graphView.nodes) { graphView.ConnectNodes(node); }
    }

    private void LoadNodeData(DialogueSO dialogue, Dictionary<string, BaseNode> createdNodes)
    {
        foreach (var nodeData in dialogue.nodesData)
        {
            BaseNode newNode = CreateNode(nodeData);
            createdNodes.Add(newNode.GUID, newNode);
            newNode.RefreshExpandedState();
        }
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
    // Depreciated
    private void LoadEdgeData(DialogueSO dialogue, Dictionary<string, BaseNode> createdNodes)
    {
        foreach (var edgeData in dialogue.edgesData)
        {
            if (createdNodes.TryGetValue(edgeData.sourceNodeGuid, out BaseNode sourceNode) &&
                createdNodes.TryGetValue(edgeData.targetNodeGuid, out BaseNode targetNode))
            {
                ConnectNodes(sourceNode, targetNode);
                Debug.Log($"Number of elements after adding edges: {graphView.graphElements.ToList().Count}");
            }
        }
    }
    

    // replaced by graphview.connectNodes?
    private void ConnectNodes(BaseNode sourceNode, BaseNode targetNode)
    {
        if (sourceNode != null && targetNode != null)
        {
            Port outputPort = (Port)sourceNode.outputContainer.ElementAt(0);
            Port inputPort = (Port)targetNode.inputContainer.ElementAt(0);

            if (outputPort != null && inputPort != null)
            {
                Edge edge = new Edge
                {
                    output = outputPort,
                    input = inputPort
                };
                outputPort.Connect(edge);
                inputPort.Connect(edge);
                graphView.AddElement(edge);
                sourceNode.RefreshPorts();
                targetNode.RefreshPorts();
            }
        }
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
        //Debug.Log($"FromBaseNode: {nodeData.nodeName} {nodeData.customNodeName} {nodeData.text} {nodeData.GUID}");

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
        //Debug.Log($"ToBaseNode: {newNode.nodeName} {newNode.customNodeName} {newNode.text} {newNode.GUID}");
        return newNode;
    }

}