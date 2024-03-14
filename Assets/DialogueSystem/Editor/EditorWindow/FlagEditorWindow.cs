using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class FlagEditorWindow : EditorWindow
{
    private List<string> flagSONames = new List<string>();
    private List<FlagSO> flagSOs = new List<FlagSO>();
    private List<string> flagSOListData = new List<string>();
    private FlagSO currentFlagSO;
    private DropdownField flagSODropdown;
    private DropdownField flagList;
    private Label nameLabel;
    private Label valueLabel;
    private Button deleteEntry;
    private TextField flagTextfield;
    private Toggle flagValue;
    private string defaultObjectEntry = "No Objects Found in Project!";

    [MenuItem("/Window/Toolkit/Dialogue System/Flag Editor")]
    public static void Open()
    {
        GetWindow<FlagEditorWindow>();
    }

    public FlagEditorWindow()
    {
        titleContent = new GUIContent("Flag Editor");
        minSize = new Vector2(500, 300);
    }


    public void OnEnable()
    {

        AddFlagOptions(); //create UI

    }

    public void OnGUI()
    {
        GetAllFlagAssets(); //get all assets from project
        GetSelectedObjectFlagsList();
        DiaplaySelectedFlagData();
    }


    public void AddFlagOptions()
    {
        flagSODropdown = new DropdownField("Flag Objects:") { choices = flagSONames };
        rootVisualElement.Add(flagSODropdown);

        flagList = new DropdownField("Flag List:") { choices = flagSOListData };
        rootVisualElement.Add(flagList);

        nameLabel = new Label();
        rootVisualElement.Add(nameLabel);

        valueLabel = new Label();
        rootVisualElement.Add(valueLabel);

        deleteEntry = new Button(() => deleteSelectedEntry()) { text = "Delete Entry" };
        rootVisualElement.Add(deleteEntry);

        VisualElement spaceElement = new VisualElement();
        spaceElement.style.height = 20;
        rootVisualElement.Add(spaceElement);

        flagTextfield = new TextField("Create New Flag:");
        rootVisualElement.Add(flagTextfield);

        flagValue = new Toggle("Flag Active: ");
        rootVisualElement.Add(flagValue);

        Button addNewFlagButton = new Button(() => AddFlagToFlagList(flagTextfield.text, flagValue.value)) { text = "Add Flag" };
        rootVisualElement.Add(addNewFlagButton);
    }



    private void GetAllFlagAssets()
    {
        string[] assetList = AssetDatabase.FindAssets("t:FlagSO"); // find all flag stores in project, returns asset GUIDs
        if (assetList.Length != 0)
        {
            flagSONames.Clear();
            flagSOs.Clear();
            foreach (string asset in assetList)
            {
                string SOpath = AssetDatabase.GUIDToAssetPath(asset); // convert GUID into asset path
                FlagSO flagSO = AssetDatabase.LoadAssetAtPath<FlagSO>(SOpath); // load asset from path
                flagSONames.Add(flagSO.name);
                flagSOs.Add(flagSO);

            }
        }
        else { flagSONames.Add(defaultObjectEntry); } //set default
    }

    private void GetSelectedObjectFlagsList()
    {
        string selectedFlagSO = flagSODropdown.value;
        if (selectedFlagSO != defaultObjectEntry)
        {
            flagSOListData.Clear();
            foreach (FlagSO flagSO in flagSOs)
            {
                if (flagSO.name == selectedFlagSO)
                {
                    currentFlagSO = flagSO;
                    foreach (FlagData flagData in currentFlagSO.flagDatas)
                    {
                        flagSOListData.Add(flagData.flagName);
                    }

                }
            }
        }

    }
    private void deleteSelectedEntry()
    {
        foreach (FlagData flagData in currentFlagSO.flagDatas)
        {
            if (flagData.flagName == flagList.value)
            {
                currentFlagSO.flagDatas.Remove(flagData);
                EditorUtility.SetDirty(currentFlagSO);
                AssetDatabase.SaveAssets();
                flagList.value = null;
                nameLabel.text = null;
                valueLabel.text = null;
                break;
            }
        }

    }

    private void DiaplaySelectedFlagData()
    {
        string selectedFlag = flagList.value;
        if (selectedFlag != null)
        {
            foreach (FlagData flagData in currentFlagSO.flagDatas)
            {
                if (flagData.flagName == selectedFlag)
                {
                    nameLabel.text = ($"Flag Name: {flagData.flagName}.");
                    valueLabel.text = ($"Flag Value: {flagData.isFlagEnabled}");

                    deleteEntry.style.display = DisplayStyle.Flex;
                }
            }
        }
        else
        {
            deleteEntry.style.display = DisplayStyle.None;
        }
    }


    private void AddFlagToFlagList(string text, bool value)
    {
        FlagData flagData = new FlagData();
        flagData.flagName = text;
        flagData.isFlagEnabled = value;
        flagData.flagDefaultState = value;
        if (currentFlagSO != null)
        {
            Debug.Log($"FlagSO {currentFlagSO.name}");
            currentFlagSO.flagDatas.Add(flagData);
            EditorUtility.SetDirty(currentFlagSO);
            AssetDatabase.SaveAssets();
        }
        else
        {
            Debug.Log("No Flag Object Selected!");
        }


    }

}
