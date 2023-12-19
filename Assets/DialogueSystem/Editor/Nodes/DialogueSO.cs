using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;



[CreateAssetMenu(menuName = "Dialogue")]

public class DialogueSO : ScriptableObject
{

    public List<NodeData> nodesData = new List<NodeData> ();
    public List<EdgeData> edgesData = new List<EdgeData>();

    //public List<EndNodeData> endNodeData = new List<EndNodeData>();
    //public List<StartNodeData> startNodeData = new List<StartNodeData>();
    //public List<DialogueNodeData> dialogueNodeData = new List<DialogueNodeData>();

    //public List<NodeData> allNodes
    //{
    //    get
    //    {
    //        List<NodeData> tmp = new List<NodeData> ();
    //        tmp.AddRange(startNodeData);
    //        tmp.AddRange(endNodeData);
    //        tmp.AddRange(dialogueNodeData);

    //        return tmp;
    //    }
    //}
}


