using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeData
{
    public string nodeType;
    public string customNodeName;
    public string GUID;
    public Vector2 graphPosition;
    public List<ChoiceData> choices;
    public string text;
}

[System.Serializable]
public class EdgeData
{
    public string sourceNodeGuid;
    public string targetNodeGuid;
}

[System.Serializable]
public class ChoiceData
{
    public int index;
    public string choice;
    public string portName; //this references port.name, not to be mistaken with port.portName which is the ports label
    public EdgeData edgeData;

}