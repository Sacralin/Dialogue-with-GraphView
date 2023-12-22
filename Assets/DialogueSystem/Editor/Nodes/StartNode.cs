using UnityEngine;
using UnityEngine.UIElements;

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
        titleContainer.style.width = 120;
        titleContainer.style.backgroundColor = new StyleColor(new Color(0.0f, 0.5f, 0.0f)); ;
        titleContainer.style.color = Color.white;
        AddOutputPort();
        RefreshExpandedState();
    }
}
