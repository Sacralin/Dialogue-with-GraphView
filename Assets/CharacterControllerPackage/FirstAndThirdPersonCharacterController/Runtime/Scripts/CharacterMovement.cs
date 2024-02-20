using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public FirstAndThirdPersonCharacterInputs inputActions;
    private CharacterController characterController;
    private Animator animator;
    
    //movement settings
    private Vector3 moveDirection;
    private Vector2 currentInput;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    private float currentSpeed;
    public float gravity = -9.81f;
    public float jumpForce = 4f;
    
    //collision mesh settings 
    private bool isCrouched;
    public float standingHeight = 1.8f;
    public float crouchedHeight = 1.3f;
    private float standingCenterY = 0.98f;
    private float crouchedCenterY = 0.7f;
    private float crouchedCenterZOffset = 0.2f;
    public float standingRadius = 0.25f;
    public float crouchedRadius = 0.47f;
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inputActions = new FirstAndThirdPersonCharacterInputs();
        inputActions.CharacterControls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleJump();
        Crouch();
    }

    private void HandleJump()
    {
        if (inputActions.CharacterControls.SpaceBar.triggered && characterController.isGrounded)
        {
            moveDirection.y = jumpForce;
            animator.SetBool("isJumping", true);
        }
    }

    //this method is called from the jump animation once the animation is complete.
    public void OnJumpAnimationCompleted()
    {
        animator.SetBool("isJumping", false);
    }

    private void HandleMovementInput()
    {
        bool runPressed = inputActions.CharacterControls.Run.ReadValue<float>() > 0; //is run pressed
        if (isCrouched) { currentSpeed = walkSpeed; } //if crouched limit current speed to walkspeed
        else { currentSpeed = runPressed ? runSpeed : walkSpeed; } // else if run pressed apply run speed, if not then apply walk speed
        
        currentInput = inputActions.CharacterControls.Walk.ReadValue<Vector2>(); // read input values from composite vector 2 inputs 
        
        float moveDirectionY = moveDirection.y; //stores Y
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.y * currentSpeed) +  //applies input from player
            (transform.TransformDirection(Vector3.right) * currentInput.x * currentSpeed);
        moveDirection.y = moveDirectionY; //reapplys Y 
        
        if (!characterController.isGrounded) { moveDirection.y += gravity * Time.deltaTime; } //apply gravity
        characterController.Move(moveDirection * Time.deltaTime); //apply movement to player character
    }

    private void Crouch()
    {
        
        if (inputActions.CharacterControls.Crouch.triggered) // if crouch is triggered switch settings 
        {
            animator.SetBool("isCrouched", !isCrouched);
            isCrouched = animator.GetBool("isCrouched"); //check if currently crouched
            characterController.height = isCrouched ? crouchedHeight : standingHeight;
            characterController.center = isCrouched ? new Vector3(0, crouchedCenterY, crouchedCenterZOffset) : new Vector3(0, standingCenterY, 0);
            characterController.radius = isCrouched ? crouchedRadius : standingRadius;
        }
    }
    
}
