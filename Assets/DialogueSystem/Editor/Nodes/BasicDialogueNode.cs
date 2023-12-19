using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class BasicDialogueNode : BaseNode
{


    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);
        nodeType = "BasicDialogue";
    }

    public override void Draw()
    {
        base.Draw();
        AddInputPort(Port.Capacity.Multi);
        AddOutputPort();
        AddDialogueBox();

        RefreshExpandedState();
    }


}
