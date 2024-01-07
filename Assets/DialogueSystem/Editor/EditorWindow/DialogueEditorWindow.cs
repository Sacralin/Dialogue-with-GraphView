using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class DialogueEditorWindow : EditorWindow
{
    private DialogueGraphView graphView;
    private DialogueSO currentDialogue;
    private TextField dialogueName;
    private NodeIO nodeIO;

    
    [MenuItem("ZTools/Dialogue System/Dialogue Graph")]
    public static void Open()
    {
        GetWindow<DialogueEditorWindow>("Dialogue Editor");
    }

    private void OnEnable()
    {
        AddGraphView();
        AddToolbar();
    }

    public void OnGUI()
    {
        graphView.Update();
    }

    private void AddGraphView()
    {
        graphView = new DialogueGraphView();
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
        nodeIO = new NodeIO(graphView);
    }

    private void AddToolbar()
    {
        Toolbar toolbar = new Toolbar();

        Button saveButton = new Button(() => { nodeIO.Save(dialogueName.value); }) { text = "Save"};
        toolbar.Add(saveButton);

        Button loadButton = new Button(() => { Debug.Log("Not Yet Implemented"); }) { text = "Load" };
        toolbar.Add(loadButton);

        Button newButton = new Button(() => { graphView.Refresh(); }) { text = "New" };
        toolbar.Add(newButton);

        Button flagsButton = new Button(() => OpenFlagWindow()) { text = "Flags" };
        toolbar.Add(flagsButton);

        Label label = new Label("FileName:");
        label.style.paddingTop = 2;
        label.style.paddingLeft = 20;
        toolbar.Add(label);

        dialogueName = new TextField() { value = "New Dialogue" };
        toolbar.Add(dialogueName);

        rootVisualElement.Add(toolbar);
    }

    

    private void OpenFlagWindow()
    {
        FlagEditorWindow flagEditorWindow = (FlagEditorWindow)GetWindow(typeof(FlagEditorWindow));
        
    }

    [OnOpenAsset(1)]
    public static bool OpenOnClick(int instanceID, int line)
    {
        Object item = EditorUtility.InstanceIDToObject(instanceID);
        if (item is DialogueSO) 
        {
            DialogueEditorWindow window = (DialogueEditorWindow)GetWindow(typeof(DialogueEditorWindow));
            window.titleContent = new GUIContent("Dialogue Editor");
            window.currentDialogue = item as DialogueSO;
            window.minSize = new Vector2(500, 300);
            window.Load();

        }

        return false;
    }

    private void Load() 
    {
        if (currentDialogue != null)
        {
            dialogueName.value = currentDialogue.name;
        }
        nodeIO.Load(currentDialogue);
        
    }

    

}


