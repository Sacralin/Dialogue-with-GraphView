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
        titleContainer.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.5f)); ;
        titleContainer.style.color = Color.white;
        AddInputPort(Port.Capacity.Multi);
        AddOutputPort();
        AddDialogueBox();

        RefreshExpandedState();
    }


}
