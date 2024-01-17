using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class DialogueSO : ScriptableObject
{
    public List<NodeDataSO> nodesData = new List<NodeDataSO> ();
    public NodeDataSO currentNode;
    
}

[System.Serializable]
public class NodeDataSO
{
    public string nodeTypeData;
    public string customNodeNameData;
    public string GUIDData;
    public Vector2 graphPositionData;
    public List<ChoiceDataSO> choicesData;
    public string textData;
    public string flagObjectData;
    public string triggerFlagData;
    public string triggerValueData;
    public string eventTypeData;
}

[System.Serializable]
public class EdgeDataSO
{
    public string sourceNodeGuidData;
    public string targetNodeGuidData;
}

[System.Serializable]
public class ChoiceDataSO
{
    public int indexData;
    public string choiceData;
    public string portNameData; //this references port.name, not to be mistaken with port.portName which is the ports label
    public EdgeDataSO edgeDataData;

}


