using System;
using System.Collections.Generic;
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

    private BaseNode CreateNode(Vector2 position, string className)
    {
        Type nodeType = Type.GetType(className);
        BaseNode node = (BaseNode)Activator.CreateInstance( nodeType );
        node.Initialize(position);
        node.Draw();
        return node;
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

    public void Save(string filename)
    {
        DialogueSO dialogueSO = ScriptableObject.CreateInstance<DialogueSO>();
        List<BaseNode> allNodes = new List<BaseNode>();
        List<Edge> allEdges = new List<Edge>();
        foreach(var node in nodes.ToList())
        {
            allNodes.Add((BaseNode)node);
            
        }
        foreach(var edge in edges.ToList())
        {
            allEdges.Add(edge);
        }
        dialogueSO.nodesData = allNodes;
        dialogueSO.edgesData = allEdges;
        AssetDatabase.CreateAsset(dialogueSO, $"Assets/DialogueSystem/Runtime/{filename}.asset"); 
        AssetDatabase.SaveAssets();
    }

    public void Load(DialogueSO dialogue)
    {
        //DeleteElements(graphElements.ToList());

        foreach (var node in dialogue.nodesData) { AddElement(node); /*node.RefreshPorts();*/ }
        foreach (var edge in dialogue.edgesData) { AddElement(edge); }//doesnt work as intended
        
    }

    
    
}
