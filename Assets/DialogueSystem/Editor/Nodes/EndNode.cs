using UnityEngine;

[System.Serializable]
public class EndNode : BaseNode
{
    public override void Initialize(Vector2 position)
    {

        base.Initialize(position);
        nodeType = "End";
    }

    public override void Draw()
    {
        base.Draw();
        AddInputPort();
        RefreshExpandedState();
    }
}

