using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueNode : BaseNode
{
    public override void Initialize(Vector2 position)
    {
        
        base.Initialize(position);
        nodeName = "Dialogue";
    }

    public override void Draw()
    {
        base.Draw();
        inputContainer.style.flexDirection = FlexDirection.Row;
        AddInputPort(Port.Capacity.Multi);
        AddOutputPort();
        
        RefreshExpandedState();
    }
}
