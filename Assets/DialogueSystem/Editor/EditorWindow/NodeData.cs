using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeData
{
    public string nodeType;
    public string customNodeName;
    public string GUID;
    public Vector2 graphPosition;
    public List<string> choices;
    public string text;
}