using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] private PlayerPhysx playerPhysx;
    [SerializeField] private PlayerVisualizer playerVisualizer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    
    [Header(" Movement ")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float airVelocity = 8f;
    [SerializeField] private float jumpForce = 15;
    [SerializeField] private bool canMove = true;
    [SerializeField] private Vector3 moveInputVector;
    private bool grounded;
    
    public static Player Instance;
    [SerializeField] public Inventory Inventory;

    [Header("Wall Mechanics")]
    private bool isWallSliding = false;
    private bool isWallJumping = false;
    private float wJumpDir;
    private float wJumpCounter;
    [SerializeField] private float wJumpTime;
    [SerializeField] private float wJumpDur;
    [SerializeField] private Vector2 wJumpForce;
    [SerializeField] private float wSlideSpeed;
    [SerializeField] private Transform wCheck, gCheck;
    [SerializeField] private LayerMask wLayer, gLayer;

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
        if (canMove)
        {
            moveInputVector = playerInput.moveVector;

            if (moveInputVector.x != 0)
            {
                MovementHandler();
            }
            else
            {
                if (playerPhysx.CurrentVelocity().y > 0)
                {
                    return;
                }
                StopMovementHandler();
            }
        }

        wallSlide();
        wallJump();

        grounded = isGrounded();
    }

    private void FixedUpdate()
    {
        if (grounded)
        {
            //Debug.Log("groundStop");
            stopWallJump();
        }

        if (isWallJumping && isWalled())
        {
            stopWallJump();
        }
    }

    private void StopMovementHandler()
    {
        StopInPlace();
        playerVisualizer.SetIdle();
    }

    private void MovementHandler()
    {
        if (!isWallJumping)
        {
            Flip();
            playerVisualizer.SetRun();
        
            playerPhysx.HandleMovement(moveInputVector, moveSpeed);
        }
    }
 
    private void JumpingHandler()
    {
        if (grounded)
        {
            playerVisualizer.SetJump();
            playerPhysx.Jump(moveInputVector, airVelocity, jumpForce);
        }

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
        
        if (moveInputVector.x != 0 && !isWallJumping)
        {
            playerVisualizer.spriteComponent.flipX = moveInputVector.x > 0;
            if (moveInputVector.x > 0)
            {
                wCheck.localPosition = new Vector3(0.25f, 0f, 0f);
            }
            else if (moveInputVector.x < 0)
            {
                wCheck.localPosition = new Vector3(-0.25f, 0f, 0f);
            }
        }
    }
    
    public void EnableMovement() => canMove = true;
    public void DisableMovement() => canMove = false;
    public void StopInPlace() => playerPhysx.HandleMovement(new Vector3(0,0,0), 0);
    
    public void Jump() => playerPhysx.Jump(moveInputVector, airVelocity, jumpForce);
    public Vector3 Velocity() => playerPhysx.CurrentVelocity();
    //public bool isGrounded() => playerPhysx.IsGrounded;

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(gCheck.position, 0.1f, gLayer);
    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wCheck.position, 0.2f, wLayer);
    }

    private void wallSlide()
    {
        //Debug.Log(isWalled() + ", " + isGrounded() + ", " + moveInputVector.x);
        if (isWalled() && !isGrounded() && moveInputVector.x != 0)
        {
            //Debug.Log("wallDetect");
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void wallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wJumpDir = -moveInputVector.x;
            wJumpCounter = wJumpTime;

            //CancelInvoke(nameof(stopWallJump));
        }
        else if (grounded)
        {
            wJumpCounter = wJumpTime;
            isWallJumping = false;
            //CancelInvoke(nameof(stopWallJump));
        }
        else
        {
            wJumpCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wJumpCounter > 0f && !isWallJumping && isWalled())
        {
            canMove = false;
            isWallJumping = true;
            //StopInPlace();
            moveInputVector = new Vector3(0f, 0f, 0f);

            rb.velocity = new Vector2(wJumpDir * wJumpForce.x, wJumpForce.y);
            wJumpCounter = 0f;

            if (moveInputVector.x != wJumpDir)
            {
                sr.flipX = !sr.flipX;
                if (sr.flipX)
                {
                    wCheck.localPosition = new Vector3(0.25f, 0f, 0f);
                }
                else if (!sr.flipX)
                {
                    wCheck.localPosition = new Vector3(-0.25f, 0f, 0f);
                }
            }

            //Invoke(nameof(stopWallJump), wJumpDur);
        }
    }

    private void stopWallJump()
    {
        Debug.Log("wjStopped");
        isWallJumping = false; 
        canMove = true;
    }
    
}