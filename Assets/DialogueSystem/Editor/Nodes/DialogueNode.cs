using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
        AddInputPort(Port.Capacity.Multi);
        AddOutputPort();
        RefreshExpandedState();
    }
}
