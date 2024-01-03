using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class DialogueNode : BaseNode
{
    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);
        nodeType = "Dialogue";
    }

    public override void Draw()
    {
        base.Draw();
        titleContainer.style.backgroundColor = new StyleColor(new Color(0.6f, 0.6f, 0.2f)); ;
        titleContainer.style.color = Color.white;
        AddAddChoiceButton();
        AddDialogueBox();
        AddInputPort(Port.Capacity.Multi);
        if(choices.Count > 0){ LoadChoicePorts(); } //load ports from exsisting data
        else{ AddChoicePort(); } // load new port
        RefreshExpandedState();
    }

    private void AddAddChoiceButton()
    {
        Button button = new Button(() => AddChoicePort()) { text = "Add Choice" };
        titleContainer.Insert(2, button);
        titleContainer.style.height = 60;
    }


    //Add choice port to output container
    private void AddChoicePort(bool isLoaded = false, ChoiceData choiceData = null)
    {
        if (choiceData == null)
            choiceData = new ChoiceData();
        Port port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        port.portName = "";
        
        TextField choiceTextField = new TextField("");
        choiceTextField.RegisterValueChangedCallback(value => { choiceData.choice = value.newValue; });
        choiceTextField.SetValueWithoutNotify(choiceData.choice);
        choiceTextField.style.width = 80;
        port.Add(choiceTextField);
        if (outputContainer.childCount > 0) {
            Button deletePortButton = new Button(() => DeletePort(port)) { text = "X" };
            port.Add(deletePortButton);
            port.style.left = 0;
        }
        if (isLoaded) { outputContainer.Insert(choiceData.index, port); }
        else {
            port.name = Guid.NewGuid().ToString();
            outputContainer.Add(port);
            choiceData.index = outputContainer.IndexOf(port);
            choiceData.portName = port.name;
            choices.Add(choiceData);
        }
    }

    //load choice ports with existing data
    public void LoadChoicePorts() 
    {
        foreach (ChoiceData choiceData in choices) { AddChoicePort(true, choiceData); }
    }

    // NOTE: port index works the same as an array, therefore with 3 ports 0,1,2 removing port 1 will leave index 0 and 2 remaning
    private void DeletePort(Port port) 
    {
        foreach (Port containerPort in outputContainer.Children().ToList()) {
            foreach (Edge edge in containerPort.connections) {
                if (edge != null) { 
                    edge.input.Disconnect(edge);
                    graphView.RemoveElement(edge);
                }
            }
        }
        graphView.ClearOldEdgeData(); // clear stored edge data
        choices.RemoveAll(choice => choice.index == outputContainer.IndexOf(port)); //remove choice from list
        outputContainer.Clear(); //clear the container
        for (int i = 0;i < choices.Count; i++){ choices[i].index = i;} //reorder choices list to remove any gaps (stop index out of range exception)
        LoadChoicePorts(); //repopulate container with data stored in choices
        graphView.ConnectNodes(this); // reconnect the nodes
    }

    




}
