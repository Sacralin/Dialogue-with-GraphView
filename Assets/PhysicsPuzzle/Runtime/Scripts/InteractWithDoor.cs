using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithDoor : MonoBehaviour
{
    public PuzzleInputs inputActions;
    public CinemachineVirtualCamera puzzleCamera;
    private InputSystemManager inputSystemManager;
    private bool puzzleActive;
    private bool inputChanged;

    // Start is called before the first frame update
    void Start()
    {
        inputSystemManager = FindAnyObjectByType<InputSystemManager>();
        inputActions = new PuzzleInputs();
        inputActions.PuzzleControls.Enable();
        puzzleActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputChanged)
        {
            if (puzzleActive)
            {
                inputSystemManager.currentState = InputSystemManager.GameState.Puzzle;
                inputChanged = false;
            }
            else
            {
                inputSystemManager.currentState = InputSystemManager.GameState.Character;
                inputChanged = false;
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (inputActions.PuzzleControls.Interact.ReadValue<float>() > 0f)
            {
                puzzleCamera.Priority = 100;
                puzzleActive = true;
                inputChanged = true;

            }

            if (inputActions.PuzzleControls.Exit.triggered)
            {
                puzzleCamera.Priority = 0;
                puzzleActive = false;
                inputChanged = true;
            }
            
        }

    }

    public void PuzzleComplete()
    {
        puzzleCamera.enabled = false;
        puzzleActive = false;
        inputChanged = true;
    }
}
