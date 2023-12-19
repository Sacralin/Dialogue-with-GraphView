using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class DialogueSO : ScriptableObject
{
    public List<NodeData> nodesData = new List<NodeData> ();
    public List<EdgeData> edgesData = new List<EdgeData>();

}


