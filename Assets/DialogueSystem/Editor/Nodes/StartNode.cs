using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : BaseNode
{


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
