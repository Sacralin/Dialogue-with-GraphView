using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InputSystemManager : MonoBehaviour
{
    //Character Controller Input Scripts
    MouseLook[] mouseLookScripts;
    CharacterMovement[] characterMovementSctipts;

    //Dialogue System Input Scripts
    DialogueManager[] dialogueManagerScripts;
    TwoDimentionalAnimationStateController[] animationStateController; // not all of these need to be arrays as there is only 1 controller.

    //Puzzle Input Scripts
    CircleMazeController[] mazeControllerScripts;

    public enum GameState
    {
        Dialogue,
        Character,
        Puzzle
    }

    
    public GameState currentState;


    // Start is called before the first frame update
    void Start()
    {
        mazeControllerScripts = FindObjectsOfType<CircleMazeController>();
        dialogueManagerScripts = FindObjectsOfType<DialogueManager>();
        mouseLookScripts = FindObjectsOfType<MouseLook>();
        characterMovementSctipts = FindObjectsOfType<CharacterMovement>();
        animationStateController = FindObjectsOfType<TwoDimentionalAnimationStateController>();
        currentState = GameState.Character;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.Dialogue:
                EnableDialogueInput();
                DisableCharacterInput();
                DisablePuzzleInput();
                break;
            case GameState.Character:
                EnableCharacterInput();
                DisableDialogueInput();
                DisablePuzzleInput();
                break;
            case GameState.Puzzle:
                EnablePuzzleInput();
                DisableDialogueInput();
                DisableCharacterInput();
                break;
        }
    }

    private void EnablePuzzleInput()
    {
        foreach(CircleMazeController mazeController in mazeControllerScripts)
        {
            mazeController.inputActions.Enable();
        }
    }

    private void DisablePuzzleInput()
    {
        foreach (CircleMazeController mazeController in mazeControllerScripts)
        {
            mazeController.inputActions.Disable();
        }
    }

    private void EnableCharacterInput()
    {
        foreach (MouseLook mouseLook in mouseLookScripts)
        {
            mouseLook.inputActions.Enable();

        }
        foreach (CharacterMovement characterMovement in characterMovementSctipts)
        {
            characterMovement.inputActions.Enable();
        }
        foreach (TwoDimentionalAnimationStateController stateController in animationStateController)
        {
            stateController.inputActions.Enable();
        }
    }

    private void DisableCharacterInput()
    {
        foreach(MouseLook mouseLook in mouseLookScripts)
        {
            mouseLook.inputActions.Disable();
            
        }
        foreach(CharacterMovement characterMovement in characterMovementSctipts)
        {
            characterMovement.inputActions.Disable();
        }
        foreach (TwoDimentionalAnimationStateController stateController in animationStateController)
        {
            stateController.inputActions.Disable();
        }
    }

    private void EnableDialogueInput()
    {
        foreach (DialogueManager dialogueManager in dialogueManagerScripts)
        {
            dialogueManager.dialogueInput.Enable();
        }

    }

    private void DisableDialogueInput()
    {
        foreach(DialogueManager dialogueManager in dialogueManagerScripts)
        {
            dialogueManager.dialogueInput.Disable();
        }

    }
}
