using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeAndFlagAssetStateManager
{
    public void ResetAllFlagAssets()
    {
        FlagNodeTools flagNodeTools = new FlagNodeTools();
        List<FlagSO> listofFlagAssets = flagNodeTools.GetAllFlagAssets();
        foreach (FlagSO flagSO in listofFlagAssets)
        {
            foreach (FlagData flagData in flagSO.flagDatas)
            {
                flagData.isFlagEnabled = flagData.flagDefaultState;
            }
        }
    }

    public void ResetAllNodeAssets()
    {
        string[] assetList = AssetDatabase.FindAssets("t:DialogueSO"); 
        if (assetList.Length != 0)
        {
            foreach (string asset in assetList)
            {
                string SOpath = AssetDatabase.GUIDToAssetPath(asset); 
                DialogueSO dialogueSO = AssetDatabase.LoadAssetAtPath<DialogueSO>(SOpath);
                dialogueSO.currentNode = new NodeDataSO();
            }
        }
        
    }


}
