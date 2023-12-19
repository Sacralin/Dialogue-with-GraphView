using UnityEditor;
using UnityEditor.Callbacks;
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

        Button loadButton = new Button(() => { Load(); }) { text = "Load" };
        toolbar.Add(loadButton);

        dialogueName = new TextField("FileName:") { value = "New Dialogue" };
        toolbar.Add(dialogueName);

        rootVisualElement.Add(toolbar);
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


