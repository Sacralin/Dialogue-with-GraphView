using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CircleMazeController : MonoBehaviour
{
    float currentAngle = 0;
    public float rotationSpeed = 0.5f;
    public PuzzleInputs inputActions;
    
    // Start is called before the first frame update
    void Start()
    {
        
        inputActions = new PuzzleInputs();
        inputActions.PuzzleControls.Enable();
    }

    // Update is called once per frame
    void Update()
    {

        if (inputActions.PuzzleControls.RotateCounterClockwise.ReadValue<float>() > 0)
        {
            currentAngle += rotationSpeed;
        }
        else if (inputActions.PuzzleControls.RotateClockwise.ReadValue<float>() > 0)
        {
            currentAngle -= rotationSpeed;
        }

        transform.localEulerAngles = new Vector3(0f, 0f, currentAngle);

    }
}
