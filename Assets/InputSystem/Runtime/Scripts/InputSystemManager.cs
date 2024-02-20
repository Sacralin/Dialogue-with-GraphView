using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InputSystemManager : MonoBehaviour
{
    ////Character Controller Input Scripts
    //MouseLook[] mouseLookScripts;
    //CharacterMovement[] characterMovementSctipts;

    ////Dialogue System Input Scripts
    //DialogueTrigger[] dialogueTriggerScripts;
    //DialogueManager[] dialogueManagerScripts;


    FirstAndThirdPersonCharacterInputs inputActions;

    public enum GameState
    {
        Dialogue,
        Character,
        Puzzle
    }

    
    public GameState currentState = GameState.Character;


    // Start is called before the first frame update
    void Start()
    {

        inputActions = new FirstAndThirdPersonCharacterInputs();
        inputActions.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.Dialogue:
                HandleDialogueInput();
                break;
            case GameState.Character:
                HandleCharacterInput();
                break;
            case GameState.Puzzle:
                HandlePuzzleInput();
                break;
        }
    }

    private void HandlePuzzleInput()
    {
        throw new NotImplementedException();
    }

    private void HandleCharacterInput()
    {
        throw new NotImplementedException();
    }

    private void HandleDialogueInput()
    {
        if (inputActions.CharacterControls.SpaceBar.triggered)
        {

        }
    }
}
