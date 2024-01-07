using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EventNode : BaseNode
{
    private List<string> flagChoices = new List<string> { "True", "False" };
    private List<string> events;
    private FlagNodeTools flagNodeTools;
    private DropdownField eventDropdown;
    private DropdownField flagAssetList;
    private DropdownField flagList;
    private DropdownField flagValue;
    private string flagEvent = "Toggle Flag Value";
    private string selectedEvent; // maybe make this a list to add multiple events


    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);
        nodeType = "Event";
        
    }
    public override void Draw()
    {
        base.Draw();
        events = new List<string>() { flagEvent };
        flagNodeTools = new FlagNodeTools();
        AddInputPort(Port.Capacity.Multi);
        AddOutputPort();
        AddEventOptions();
        if(flagObject != null)
        {
            AddFlagEventUI();
            eventDropdown.value = eventType;
            flagAssetList.value = flagObject;
            flagList.value = triggerFlag;
            flagValue.value = triggerValue;
            selectedEvent = eventType;
        }
        

    }

    public override void Update()
    {
        base.Update();
        PopulateFlagEventLists();
    }
    private void AddEventOptions()
    {
        eventDropdown = new DropdownField("Event:") { choices = events};
        eventDropdown.RegisterValueChangedCallback(OnDropdownValueChanged);
        mainContainer.Add(eventDropdown);
    }

    void OnDropdownValueChanged(ChangeEvent<string> evt)
    {
        string selectedValue = evt.newValue;
        if(selectedValue == flagEvent) { AddFlagEventUI(); selectedEvent = selectedValue; }
    }

    private void AddFlagEventUI()
    {
        flagAssetList = new DropdownField("Flag Asset:");
        mainContainer.Add(flagAssetList);
        
        flagList = new DropdownField("Trigger Flag:");
        mainContainer.Add(flagList);

        flagValue = new DropdownField("Set Value To:") { choices = flagChoices };
        mainContainer.Add(flagValue);
    }

    private void PopulateFlagEventLists()
    {
        if(selectedEvent == flagEvent)
        {
            flagAssetList.choices = flagNodeTools.GetNamesOfFlagAssets(flagNodeTools.GetAllFlagAssets());
            flagList.choices = flagNodeTools.GetSelectedObjectFlagsList(flagNodeTools.GetAllFlagAssets(), flagAssetList.value);
            eventType = eventDropdown.value;
            flagObject = flagAssetList.value;
            triggerFlag = flagList.value;
            triggerValue = flagValue.value;
        }
    }


}
