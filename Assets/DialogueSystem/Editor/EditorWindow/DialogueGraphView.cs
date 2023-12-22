using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


public class DialogueGraphView : GraphView
{
    //private List<Edge> previousEdges = new List<Edge>();

    public DialogueGraphView()
    {
        graphViewChanged += OnGraphViewChanged;
        //previousEdges.AddRange(edges.ToList());
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
        this.AddManipulator(CreateNodeContextualMenu("Add Node (BasicDialogue)", "BasicDialogueNode"));
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
        node.GraphView(this);
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


    //sets the compatible ports from node to node, stops ports from attaching to the same node or same node type e.g. output to output
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

    //Clears the graphview ready for a new graph, runs when "New" is clicked, misleading method name
    public void Refresh() 
    {
        foreach (var edge in edges.ToList()) { RemoveElement(edge); } //clear all edges from graphview
        foreach (var node in nodes.ToList()) { RemoveElement(node); } //clear all edges from graphview
        BaseNode startNode = CreateNode(new Vector2(50f, 50f), "StartNode"); //create a start node
        AddElement(startNode); //add startnode
    }

        
    //Connects nodes baised on port GUID and edgeData stored in choices
    public void ConnectNodes(BaseNode node)
    {
        foreach(ChoiceData choice in node.choices) { //itterate over choices
            if (choice.edgeData != null && choice.edgeData.sourceNodeGuid != null && choice.edgeData.targetNodeGuid != null) { //check if edge data is present
                BaseNode targetNode = new BaseNode(); //target node container
                foreach(BaseNode nodes in nodes.ToList()) { //get all basenodes in nodes(graphview)
                    if(choice.edgeData.targetNodeGuid == nodes.GUID) { //find targetnode in graphview
                        targetNode = nodes; //store targetnode
                    }
                }
                Port inputPort = targetNode.inputContainer.Children().OfType<Port>().FirstOrDefault();
                //Port inputPort = (Port)targetNode.inputContainer.ElementAt(0); //nodes only have 1 input so we assign that
                Port outputPort = (Port)node.outputContainer.ElementAt(choice.index); //assign subject nodes output based on data stored in choices
                if (outputPort != null && inputPort != null) { //null check
                    Edge edge = new Edge { output = outputPort, input = inputPort }; //create the new edge and assign ports
                    outputPort.Connect(edge); //connect
                    inputPort.Connect(edge); //connect
                    AddElement(edge); //add element to graphview
                    node.RefreshPorts(); //refresh ports
                    targetNode.RefreshPorts(); //refresh ports
                }
                
            }
        }
    }

    //updates node links when they are created - ClearOldEdgeData() handles removing them
    //this makes it easier to handle links when choices are deleted from dialogue nodes
    //this is because we redraw the output container to avoid null references when comparing the choices list to the port array.
    private GraphViewChange OnGraphViewChanged(GraphViewChange changes)
    {
        // Check for connected edges
        if (changes.edgesToCreate != null && changes.edgesToCreate.Count > 0)
        {
            foreach (var edge in changes.edgesToCreate)
            {
                // Populate edgedata on parent nodes
                BaseNode input = (BaseNode)edge.input.node;
                BaseNode output = (BaseNode)edge.output.node;
                foreach(ChoiceData choice in output.choices) {
                    if(choice.portName == edge.output.name) {
                        EdgeData edgeData = new EdgeData();
                        edgeData.sourceNodeGuid = output.GUID; 
                        edgeData.targetNodeGuid = input.GUID;
                        choice.edgeData = edgeData;
                    }
                }
            }
        }
        return changes;
    }

    //clears edge data stored in choices - runs on Save() and deletePort()
    //using this instead of the ongraphchanged callback because the callback doesnt run when the edge is deleted for some reason 
    public void ClearOldEdgeData()
    {
        foreach(BaseNode node in nodes) {
            var outputPorts = node.outputContainer.Query<Port>().ToList();
            foreach (ChoiceData choice in node.choices) {
                foreach (var outputPort in outputPorts) {
                    if (outputPort.name == choice.portName) {
                        if(!outputPort.connected) {
                            choice.edgeData = new EdgeData();
                        }
                    }
                }
            }
        }
    }

    

}
