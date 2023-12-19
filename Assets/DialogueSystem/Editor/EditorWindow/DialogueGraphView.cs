using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    public DialogueGraphView()
    {
        
        AddManipulators();
        AddGridBackground();
        AddStyles();
        BaseNode startNode = CreateNode(new Vector2(50f,50f), "StartNode");
        AddElement(startNode);
        
    }

    private void AddManipulators()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Dialogue)", "DialogueNode"));
    }

    private IManipulator CreateNodeContextualMenu(string actionTitle, string className)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(actionEvent.eventInfo.localMousePosition, className))));
            
        return contextualMenuManipulator;
    }

    private BaseNode CreateNode(Vector2 position, string classname)
    {
        Type nodeType = Type.GetType(classname);
        BaseNode node = (BaseNode)Activator.CreateInstance(nodeType);
        node.Initialize(position);
        node.Draw();
        return node;
    }

    private BaseNode CreateNode(NodeData nodeData)
    {
        BaseNode newNode = ToBaseNode(nodeData);
        Debug.Log($"Before Int+Draw: {newNode.nodeName} {newNode.customNodeName} {newNode.text} {newNode.GUID}");
        //newNode.Initialize(nodeData.graphPosition);
        newNode.SetPosition(new Rect(newNode.graphPosition, Vector2.zero));
        newNode.Draw();
        AddElement(newNode);
        Debug.Log($"Post Int+Draw: {newNode.nodeName} {newNode.customNodeName} {newNode.text} {newNode.GUID}");

        return newNode;
    }

    private void AddGridBackground()
    {
        GridBackground grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
    }

    private void AddStyles()
    {
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/DialogueSystem/Editor/StyleSheets/GraphViewStyles.uss");
        styleSheets.Add(styleSheet);
        //add node styles later
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach (port =>
        {
            if(startPort == port) { return; }
            if(startPort.node == port.node) { return; }
            if(startPort.direction == port.direction) { return; }
            compatiblePorts.Add(port);
        }) ;
        return compatiblePorts;
    }


    // saving and loading nodes and edges
    public void Save(string filename)
    {
        DialogueSO dialogueSO = ScriptableObject.CreateInstance<DialogueSO>();
        
        dialogueSO.nodesData = SaveNodes().ToList();
        dialogueSO.edgesData = SaveEdges().ToList();

        AssetDatabase.CreateAsset(dialogueSO, $"Assets/DialogueSystem/Runtime/{filename}.asset");
        AssetDatabase.SaveAssets();
    }

    public List<NodeData> SaveNodes()
    {
        List<NodeData> allNodes = new List<NodeData>();
        foreach (var node in nodes.ToList())
        {
            NodeData nodeData = FromBaseNode((BaseNode)node);
            allNodes.Add(nodeData);
        }
        return allNodes;
    }

    private List<EdgeData> SaveEdges()
    {
        List<EdgeData> allEdges = new List<EdgeData>();
        Edge[] connectedEdges = edges.Where(edge => edge.input.node != null).ToArray();
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
        DeleteElements(graphElements.ToList());
        Dictionary<string, BaseNode> createdNodes = new Dictionary<string, BaseNode>();
        LoadNodeData(dialogue, createdNodes);
        LoadEdgeData(dialogue, createdNodes);
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

    private void LoadEdgeData(DialogueSO dialogue, Dictionary<string, BaseNode> createdNodes)
    {
        foreach (var edgeData in dialogue.edgesData)
        {
            if (createdNodes.TryGetValue(edgeData.sourceNodeGuid, out BaseNode sourceNode) &&
                createdNodes.TryGetValue(edgeData.targetNodeGuid, out BaseNode targetNode))
            {
                ConnectNodes(sourceNode, targetNode);
                Debug.Log($"Number of elements after adding edges: {graphElements.ToList().Count}");
            }
        }
    }


    private void ConnectNodes(BaseNode sourceNode, BaseNode targetNode)
    {
        if (sourceNode != null && targetNode != null)
        {
            Port outputPort = sourceNode.outputContainer.ElementAt(0) as Port;
            Port inputPort = targetNode.inputContainer.ElementAt(0) as Port;

            if (outputPort != null && inputPort != null)
            {
                Edge edge = new Edge
                {
                    output = outputPort,
                    input = inputPort
                };
                outputPort.Connect(edge);
                inputPort.Connect(edge);
                AddElement(edge);
                sourceNode.RefreshPorts();
                targetNode.RefreshPorts();
            }
        }
    }

    // switch data type for storage and instansing 
    public static NodeData FromBaseNode(BaseNode baseNode)
    {
        NodeData nodeData = new NodeData();
        nodeData.nodeName = baseNode.nodeName;
        nodeData.customNodeName = baseNode.customNodeName;
        nodeData.choices = baseNode.choices;
        nodeData.text = baseNode.text;
        nodeData.GUID = baseNode.GUID;
        nodeData.graphPosition = baseNode.graphPosition;
        Debug.Log($"FromBaseNode: {nodeData.nodeName} {nodeData.customNodeName} {nodeData.text} {nodeData.GUID}");

        return nodeData;
    }

    public BaseNode ToBaseNode(NodeData nodeData)
    {
        BaseNode newNode;
        switch (nodeData.nodeName)
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
            default:
                newNode = new BaseNode();
                break;
        }
        newNode.nodeName = nodeData.nodeName;
        newNode.customNodeName = nodeData.customNodeName;
        newNode.choices = nodeData.choices;
        newNode.text = nodeData.text;
        newNode.GUID = nodeData.GUID;
        newNode.graphPosition = nodeData.graphPosition;
        //Debug.Log($"ToBaseNode: {newNode.nodeName} {newNode.customNodeName} {newNode.text} {newNode.GUID}");
        return newNode;
    }

}
