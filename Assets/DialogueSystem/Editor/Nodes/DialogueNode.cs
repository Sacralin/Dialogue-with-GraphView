using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class DialogueNode : BaseNode
{
    List<BaseNode> children = new List<BaseNode>();

    public override void Initialize(Vector2 position)
    {
        
        base.Initialize(position);
        nodeName = "Dialogue";
    }

    public override void Draw()
    {
        base.Draw();
        AddInputPort(Port.Capacity.Multi);
        AddOutputPort();
        
        RefreshExpandedState();
    }
}
