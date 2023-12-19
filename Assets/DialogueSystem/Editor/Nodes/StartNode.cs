using UnityEngine;

[System.Serializable]
public class StartNode : BaseNode
{
    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);
        nodeType = "Start";
    }

    public override void Draw()
    {
        base.Draw();
        AddOutputPort();
        RefreshExpandedState();
    }
}
