using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeInspector : EditorWindow
{
    Label typeLabel;
    Label customLabel;
    Label GUIDLabel;
    DropdownField choicesDropdown;
    Label sourceLabel;
    Label targetLabel;
    BaseNode currentNode;

    [MenuItem("ZTools/Dialogue System/Node Inspector")]
    public static void Open()
    {
        GetWindow<NodeInspector>();
    }


    public void OnEnable()
    {

        AddUI();
    }

    public void DisplayNode(BaseNode node)
    {
        List<string> choicesToString = new List<string>();
        if (node != currentNode)
        { choicesToString.Clear(); }

        currentNode = node;
        typeLabel.text = $"{typeLabel.name} {node.nodeType}";
        customLabel.text = $"{customLabel.name} {node.customNodeName}";
        GUIDLabel.text = $"{GUIDLabel.name} {node.GUID}";
        foreach (ChoiceData choice in node.choices)
        {
            choicesToString.Add(choice.index.ToString());
        }
        choicesDropdown.choices = choicesToString;
        if (choicesDropdown.value == null) { choicesDropdown.value = choicesToString[0]; }
        if (choicesDropdown.value != null)
        {
            sourceLabel.text = $"{sourceLabel.name} {node.choices[int.Parse(choicesDropdown.value)].edgeData.sourceNodeGuid}";
            targetLabel.text = $"{targetLabel.name} {node.choices[int.Parse(choicesDropdown.value)].edgeData.targetNodeGuid}";
        }


    }

    

    private void AddUI()
    {
        typeLabel = new Label() { name = "Node Type: " };
        rootVisualElement.Add(typeLabel);

        customLabel = new Label() { name = "Node Custom: " };
        rootVisualElement.Add(customLabel);

        GUIDLabel = new Label() { name = "Node GUID: " };
        rootVisualElement.Add(GUIDLabel);

        choicesDropdown = new DropdownField("Choices: " );
        rootVisualElement.Add(choicesDropdown);

        sourceLabel = new Label() { name = "Source Node: " };
        rootVisualElement.Add(sourceLabel);

        targetLabel = new Label() { name = "Target Node: " };
        rootVisualElement.Add(targetLabel);

    }
}
