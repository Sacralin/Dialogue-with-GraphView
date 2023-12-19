using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartNode : BaseNode
{

    public BaseNode child;

    public override void Initialize(Vector2 position)
    {
        
        base.Initialize(position);
        nodeName = "Start";
    }

    public override void Draw()
    {
        base.Draw();
        AddOutputPort();
        RefreshExpandedState();
    }
}