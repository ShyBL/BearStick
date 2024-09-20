using UnityEngine;
using UnityEngine.Serialization;

public class Player : OurMonoBehaviour
{
    public static Player Instance;
    [Header(" Components ")]
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] public PlayerPhysx playerPhysx;
    [SerializeField] public PlayerVisualizer playerVisualizer;
    public PlayerStateMachine playerStateMachine;
    public AudioManager AudioManager => GameManager.AudioManager;
    [Header(" Movement ")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float sprintMultiplier = .5f;
    [SerializeField] private float airVelocity = 8f;
    [SerializeField] public float jumpForce = 15;
    [SerializeField] public Vector2 wallJumpForce = new Vector2(3.5f, 6);
    [SerializeField] public int wallJumpCounter = 0; //use in the future, not sure how to handle that yet
    [SerializeField] public float wallSlideSpeed = 8f;

    [SerializeField] public bool canMove = true;
    [SerializeField] public Vector3 moveInputVector;
    
    [Header(" Inventory ")]
    [SerializeField] public Inventory inventory;
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
        MovementHandler();
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
        if (IsGrounded())
        {
            Jump();
        }
    }
    
    public void SetPlayerSpawn(Vector2 spawn)
    {
        this.gameObject.transform.position = spawn;
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
            facingDirection = moveInputVector.x > 0 ? 1 : -1;
            playerVisualizer.spriteComponent.flipX = moveInputVector.x > 0;
        }
    }
    
    public void EnableMovement() => canMove = true;
    public void DisableMovement() => canMove = false;
    public void StopInPlace() => playerPhysx.HandleMovement(new Vector3(0,0,0), 0);
    
    public void Jump() => playerPhysx.Jump(moveInputVector, airVelocity, jumpForce);
    public Vector3 Velocity() => playerPhysx.CurrentVelocity();
    public bool IsGrounded() => playerPhysx.IsGrounded;
    
    // Rect rect = new Rect(0, 0, 300, 100);
    // Vector3 offset = new Vector3(0f, 0f, 0.5f);
    
    // shows during play mode, the name of the current state, on the player
    // void OnGUI()
    // {
    //     Vector3 point = Camera.main.WorldToScreenPoint(transform.position + offset);
    //     rect.x = point.x;
    //     rect.y = Screen.height - point.y - rect.height; // bottom left corner set to the 3D point
    //     GUI.Label(rect, playerStateMachine.currentState.ToString()); // display its name, or other string
    // }
}