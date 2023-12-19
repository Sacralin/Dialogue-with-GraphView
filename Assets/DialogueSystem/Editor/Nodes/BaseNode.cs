using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class BaseNode : Node
{
    public string nodeName;
    public string customNodeName;
    public List<string> choices;
    public string text;
    public string GUID;
    public Vector2 graphPosition;

    

    

    public virtual void Initialize(Vector2 position)
    {
        nodeName = "NodeType";
        customNodeName = "CustomNodeName";
        choices = new List<string>();
        text = "Dialogue Text";
        SetPosition(new Rect(position, Vector2.zero));
        GUID = Guid.NewGuid().ToString();
        this.RegisterCallback<GeometryChangedEvent>(evt => graphPosition = GetPosition().position);
    }

    public virtual void Draw()
    {
        //style.width = 200f;
        //Title container 
        titleContainer.style.flexDirection = FlexDirection.Column;
        Label nodeLabel = new Label() { text = nodeName};
        titleContainer.Insert(0, nodeLabel);

        TextField customNodeNameTextField = new TextField("");
        customNodeNameTextField.RegisterValueChangedCallback(value => { customNodeName = value.newValue; });
        customNodeNameTextField.SetValueWithoutNotify(customNodeName);
        titleContainer.Insert(1, customNodeNameTextField);
    }

    public void AddOutputPort()
    {
        Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        outputPort.portName = "Output";
        outputContainer.Add(outputPort);
    }

    public void AddInputPort(Port.Capacity capacity = Port.Capacity.Single)
    {
        Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        inputPort.portName = "Input";
        inputContainer.Add(inputPort);
    }

}

