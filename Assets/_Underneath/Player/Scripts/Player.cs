using System;
//using FMOD.Studio;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] public PlayerPhysx playerPhysx;
    [SerializeField] public PlayerVisualizer playerVisualizer;
    public PlayerStateMachine playerStateMachine;
    
    [Header(" Movement ")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float airVelocity = 8f;
    [SerializeField] private float jumpForce = 15;
    [SerializeField] public bool canMove = true;
    [SerializeField] public Vector3 moveInputVector;
    
    
    [SerializeField] public Inventory Inventory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerStateMachine = new PlayerStateMachine();
        playerStateMachine.Initialize();
    }

    private void OnEnable()
    {
        playerInput.onMove += MovementHandler;
        playerInput.onMoveStopped += MovementHandler;
        playerInput.onJump += JumpingHandler;
    }

    private void OnDisable()
    {
        playerInput.onMove -= MovementHandler;
        playerInput.onMoveStopped -= MovementHandler;
        playerInput.onJump -= JumpingHandler;
    }

    private void Update()
    {
        playerStateMachine.currentState.Update();
        
        
    }

    private void StopMovementHandler()
    {
        StopInPlace();
        playerVisualizer.SetIdle();
    }

    private void MovementHandler()
    {
        if (canMove)
        {
            moveInputVector = playerInput.moveVector;
            Flip();
            playerPhysx.HandleMovement(moveInputVector, moveSpeed);
        }
    }
 
    private void JumpingHandler()
    {
        playerPhysx.Jump(moveInputVector, airVelocity, jumpForce);
        // if (isGrounded())
        // {
        //     Jump();
        // }
    }
    
    public int facingDirection;
    
    public void Flip() 
    {
        // change the x axis of the scale to -1 if its facing left and 1 if its facing right
        // if (moveInputVector.x != 0)
        // {
        //     facingDirection = moveInputVector.x > 0 ? 1 : -1;
        //     transform.localScale = new Vector3(1 * facingDirection, 1, 1);
        // }
        
        if (moveInputVector.x != 0)
        {
            playerVisualizer.spriteComponent.flipX = moveInputVector.x > 0;
        }
    }
    
    public void EnableMovement() => canMove = true;
    public void DisableMovement() => canMove = false;
    public void StopInPlace() => playerPhysx.HandleMovement(new Vector3(0,0,0), 0);
    
    public void Jump() => playerPhysx.Jump(moveInputVector, airVelocity, jumpForce);
    public Vector3 Velocity() => playerPhysx.CurrentVelocity();
    public bool isGrounded() => playerPhysx.IsGrounded;
    
}