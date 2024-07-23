using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] private PlayerPhysx playerPhysx;
    [SerializeField] private PlayerVisualizer playerVisualizer;
    
    [Header(" Movement ")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float airVelocity = 8f;
    [SerializeField] private float jumpForce = 15;
    [SerializeField] private bool canMove = true;
    [SerializeField] private Vector3 moveInputVector;
    
    public static Player Instance;
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
        playerVisualizer.SetIdle();
    }

    private void OnEnable()
    {
        playerInput.onMove += MovementHandler;
        playerInput.onMoveStopped += MovementHandler;
        playerInput.onJump += HandleJumping;
    }

    private void OnDisable()
    {
        playerInput.onMove -= MovementHandler;
        playerInput.onMoveStopped -= MovementHandler;
        playerInput.onJump -= HandleJumping;
    }

    private void Update()
    {
        if (canMove)
        {
            moveInputVector = playerInput.moveVector;

            if (moveInputVector.x != 0)
            {
                MovementHandler();
            }
            else
            {
                StopMovementHandler();
            }
        }
    }

    private void StopMovementHandler()
    {
        StopInPlace();
        playerVisualizer.SetIdle();
    }

    private void MovementHandler()
    {
        Flip();
        playerVisualizer.SetRun();
        playerPhysx.HandleMovement(moveInputVector, moveSpeed);
        
    }

    private void HandleJumping()
    {
        Jump();

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