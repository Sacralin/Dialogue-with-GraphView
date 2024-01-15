using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class EndNode : BaseNode
{
    private DropdownField endOptionDropdown;
    private DropdownField selectedEndDropdown;
    private bool returnOptionsAdded;
    private List<string> endOptions = new List<string>() { "Do Nothing", "Return To Node" };
    
    
    public override void Initialize(Vector2 position)
    {

        base.Initialize(position);
        nodeType = "End";
    }

    public override void Draw()
    {
        base.Draw();
        titleContainer.style.width = 200;
        mainContainer.style.width = 200;
        AddInputPort(Port.Capacity.Multi);
        RefreshExpandedState();
        AddEndOptions();
        returnOptionsAdded = false;
        endOptionDropdown.value = eventType;
        if(eventType == "Return To Node")
        {
            ReturnToNodeSelected();
            selectedEndDropdown.value = triggerFlag;
            returnOptionsAdded = true;
        }
        
        
    }

    public override void Update()
    {
        base.Update();
        if (endOptionDropdown != null)
        {
            if (endOptionDropdown.value == "Return To Node" && !returnOptionsAdded)
            {
                ReturnToNodeSelected();
                returnOptionsAdded = true;
            }
            else if (endOptionDropdown.value == "Do Nothing" && returnOptionsAdded)
            {
                mainContainer.Remove(selectedEndDropdown);
                returnOptionsAdded = false;
            }
        }
        
        if(returnOptionsAdded)
        {
            selectedEndDropdown.choices = ListOfNodes();
            eventType = endOptionDropdown.value;
            triggerFlag = selectedEndDropdown.value;

        }
        else
        {
            eventType = "Do Nothing";
            triggerFlag = "";
        }
        
        
    }

    private void AddEndOptions()
    {
        endOptionDropdown = new DropdownField("End Options") { choices = endOptions, value = endOptions[0] };
        
        mainContainer.Add(endOptionDropdown);
        
    }

    private void ReturnToNodeSelected()
    {
        selectedEndDropdown = new DropdownField("Select Node by GUID");
        mainContainer.Add(selectedEndDropdown);
    }

    private List<string> ListOfNodes()
    {
        List<string> listOfNodeGUID = new List<string>();
        foreach (BaseNode node in graphView.nodes)
        {
            listOfNodeGUID.Add(node.GUID);
        }
        return listOfNodeGUID;
    }
}

