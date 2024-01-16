using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flag List")]
public class FlagSO : ScriptableObject
{
    public List<FlagData> flagDatas = new List<FlagData>();

}



[Serializable]
public class FlagData
{
    public string flagName;
    public bool isFlagEnabled;
    public bool flagDefaultState;
}
