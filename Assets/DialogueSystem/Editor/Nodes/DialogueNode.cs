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
        AddAddChoiceButton();
        AddDialogueBox();
        AddInputPort(Port.Capacity.Multi);
        AddChoicePort();
        
        RefreshExpandedState();
    }

    public void AddAddChoiceButton()
    {
        Button button = new Button(() => AddChoicePort()) { text = "Add Choice" };
        titleContainer.Insert(2, button);
        titleContainer.style.height = 60;

        
    }

    public void AddChoicePort()
    {
        Port port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        port.portName = "";

        TextField choiceTextField = new TextField("");
        choiceTextField.RegisterValueChangedCallback(value => { text = value.newValue; });
        choiceTextField.SetValueWithoutNotify(text);
        choiceTextField.style.width = 80;
        port.Add(choiceTextField);

        if (outputContainer.childCount > 0)
        {
            Button deletePortButton = new Button(() => DeletePort(port)) { text = "X" };
            port.Add(deletePortButton);
            port.style.left = 0;
        }

        outputContainer.Add(port);
    }

    private void DeletePort(Port port) // NOTE: port index works the same as an array, therefore with 3 ports 0,1,2 removing port 1 will leave index 0 and 2 remaning
    {
        //List<Edge> edgesToRemove = port.connections.ToList();
        //graphView.RemoveElements(edgesToRemove);
        //RemoveEdgesConnectedToPort(port);
        outputContainer.Remove(port);
    }


}
