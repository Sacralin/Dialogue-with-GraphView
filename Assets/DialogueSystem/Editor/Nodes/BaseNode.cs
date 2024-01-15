using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class BaseNode : Node
{
    protected DialogueGraphView graphView;
    public string nodeType;
    public string customNodeName;
    public List<ChoiceData> choices;
    public string text;
    public string GUID;
    public Vector2 graphPosition;
    public string flagObject;
    public string triggerFlag;
    public string triggerValue;
    public string eventType;
    
    public virtual void Initialize(Vector2 position)
    {
        
        nodeType = "NodeType";
        customNodeName = "CustomNodeName";
        choices = new List<ChoiceData>();
        
        text = "Dialogue Text";
        SetPosition(new Rect(position, Vector2.zero));
        GUID = Guid.NewGuid().ToString();
        this.RegisterCallback<GeometryChangedEvent>(evt => graphPosition = GetPosition().position);
    }

    public virtual void Draw()
    {
        titleContainer.style.width = 210;
        //Title container 
        titleContainer.style.flexDirection = FlexDirection.Column;
        Label nodeLabel = new Label() { text = nodeType };
        titleContainer.Insert(0, nodeLabel);

        TextField customNodeNameTextField = new TextField("");
        customNodeNameTextField.RegisterValueChangedCallback(value => { customNodeName = value.newValue; });
        customNodeNameTextField.SetValueWithoutNotify(customNodeName);
        titleContainer.Insert(1, customNodeNameTextField);
    }

    public virtual void Update()
    {

    }

    public void GraphView(DialogueGraphView dialogueGraphView)
    {
        graphView = dialogueGraphView;
    }

    public void AddOutputPort(string portName = "Output")
    {
        
        Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        outputPort.portName = portName;
        outputPort.name = Guid.NewGuid().ToString();
        outputContainer.Add(outputPort);
        if (choices == null || choices.Count == 0)
        {
            ChoiceData data = new ChoiceData();
            data.index = 0;
            data.portName = outputPort.name;
            choices.Add(data);
        }

        if (portName == "False") //this is the second port on a flagNode - need to tidy this up coz feels messy 
        {
            ChoiceData data = new ChoiceData();
            data.index = 1;
            data.portName = outputPort.name;
            choices.Add(data);
        }

    }

    public void AddInputPort(Port.Capacity capacity = Port.Capacity.Single)
    {
        Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, capacity, typeof(bool));
        inputPort.portName = "Input";
        inputContainer.Add(inputPort);
       
    }

    public void AddDialogueBox()
    {
        TextField dialogueTextField = new TextField("");
        dialogueTextField.RegisterValueChangedCallback(value => { text = value.newValue; });
        dialogueTextField.SetValueWithoutNotify(text);
        dialogueTextField.multiline = true;
        dialogueTextField.style.height = 50;
        mainContainer.Add(dialogueTextField);
        
    }

    

    
}

