using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.PackageManager.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class DialogueEditorWindow : EditorWindow
{
    DialogueGraphView graphView;
    DialogueSO currentDialogue;
    TextField dialogueName;
    //private static int windowCount = 0;

    [MenuItem("ZTools/Dialogue System/Dialogue Graph")]
    public static void Open()
    {
        GetWindow<DialogueEditorWindow>("Dialogue Editor");
        //DialogueEditorWindow window = ScriptableObject.CreateInstance<DialogueEditorWindow>();
        //window.titleContent = new GUIContent("My Window " + windowCount);
        //window.Show();
        //windowCount++;
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
        
    }

    private void AddToolbar()
    {
        Toolbar toolbar = new Toolbar();

        Button saveButton = new Button(() => { graphView.Save(dialogueName.value); }) { text = "Save"};
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
            
            //DialogueEditorWindow window = ScriptableObject.CreateInstance<DialogueEditorWindow>();
            //window.titleContent = new GUIContent("My Window " + windowCount);
            //window.Show();
            //windowCount++;
            window.titleContent = new GUIContent("Dialogue Editor");
            window.currentDialogue = item as DialogueSO;
            window.minSize = new Vector2(500, 300);
            window.Load();

        }

        return false;
    }

    private void Load()
    {
        graphView.DeleteElements(graphView.graphElements.ToList());
        if (currentDialogue != null)
        {
            dialogueName.value = currentDialogue.name;
        }
        graphView.Load(currentDialogue);
        
    }






}


