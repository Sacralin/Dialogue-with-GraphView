using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;



[CreateAssetMenu(menuName = "Dialogue")]
[System.Serializable]
public class DialogueSO : ScriptableObject
{

    public List<BaseNode> nodesData = new List<BaseNode> ();
    public List<Edge> edgesData = new List<Edge>();
}


