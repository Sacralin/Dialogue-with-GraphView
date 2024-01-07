using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Profiling.RawFrameDataView;

public class FlagNode : BaseNode
{
    private List<string> flagSONames = new List<string>();
    private List<FlagSO> flagSOs = new List<FlagSO>();
    private List<string> flagSOListData = new List<string>();
    private FlagNodeTools flagNodeTools;
    private FlagSO currentFlagSO;
    private DropdownField flagObjectDropdown;
    private DropdownField triggerFlagDropdown;
    private DropdownField triggerValueDropdown;
    private string defaultObjectEntry = "No Objects Found in Project!";


    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);
        nodeType = "Flag";
    }

    public override void Draw()
    {
        base.Draw();
        AddInputPort(Port.Capacity.Multi);
        AddOutputPort();
        AddFlagField();
        flagNodeTools = new FlagNodeTools();
        flagObjectDropdown.value = flagObject;
        triggerFlagDropdown.value = triggerFlag;
        triggerValueDropdown.value = triggerValue;
        RefreshExpandedState();
    }

    public override void Update()
    {
        base.Update();
        PopulateFlagEventLists();
        SetTriggerValue();
        flagObject = flagObjectDropdown.value;
        triggerFlag = triggerFlagDropdown.value;
        triggerValue = triggerValueDropdown.value;
    }


    public void AddFlagField()
    {
        flagObjectDropdown = new DropdownField("Flag Object");
        mainContainer.Add(flagObjectDropdown);
        triggerFlagDropdown = new DropdownField("Trigger Flag:");
        mainContainer.Add(triggerFlagDropdown);
        triggerValueDropdown = new DropdownField("Trigger Value:");
        mainContainer.Add(triggerValueDropdown);

    }

    private void PopulateFlagEventLists()
    {
        flagObjectDropdown.choices = flagNodeTools.GetNamesOfFlagAssets(flagNodeTools.GetAllFlagAssets());
        triggerFlagDropdown.choices = flagNodeTools.GetSelectedObjectFlagsList(flagNodeTools.GetAllFlagAssets(), flagObjectDropdown.value);
        flagObject = flagObjectDropdown.value;
        triggerFlag = triggerFlagDropdown.value;
        triggerValue = triggerValueDropdown.value;
        
    }

    private void SetTriggerValue()
    {
        string selectedFlag = triggerFlagDropdown.value;
        if(!string.IsNullOrEmpty(selectedFlag))
        {
            List<string> choices = new List<string> { "True", "False" };
            triggerValueDropdown.choices = choices;
        }
        else
        {
            triggerValueDropdown.choices.Clear();
        }
    }
}
