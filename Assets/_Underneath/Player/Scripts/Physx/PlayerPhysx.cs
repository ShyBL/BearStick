using UnityEngine;

public class PlayerPhysx : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [Header(" RB Settings ")]
    [SerializeField] private Rigidbody2D rb;
    
    [Header(" Ground Detection ")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector2 groundCheckBoxSize;
    [SerializeField] private float groundCastDistance;
    
    public bool IsGrounded => 
        Physics2D.BoxCast(
        transform.position, 
        groundCheckBoxSize, 
        0, 
        -transform.up, 
        groundCastDistance, 
        whatIsGround
        // Physics.BoxCast(
        // playerTransform.position,
        // groundCheckBoxSize / 2,
        // -playerTransform.up,
        // out _,
        // playerTransform.rotation,
        // groundCastDistance,
        // whatIsGround
    );
    
    public void HandleMovement(Vector3 movement, float speed)
    {
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y,movement.z * speed);
    }
    
    public void EnableGravity()
    {
        rb.gravityScale = 4f;
    }
    
    public void DisableGravity()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }
    
    public Vector3 CurrentVelocity() => new Vector3(rb.velocity.x, rb.velocity.y);
    public void SetVelocity(float vX, float vY) => rb.velocity = new Vector2(vX, vY);
    
    public void Jump(Vector3 jumpVector, float airVelocity, float jumpForce)
    {
        bIsJumping = true;
        rb.velocity = new Vector3(jumpVector.x * airVelocity, jumpForce,jumpVector.z * airVelocity);
    }
    
    [Header(" Wall Collision ")]
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private Vector2 wallCheckBoxSize;
    [SerializeField] private float wallCastDistance;
    
    [Header(" Ledge Rays Parameters ")]
    private bool ledgeRaysEnabled = true;
    [SerializeField] private float ledgeRaysDistance;
    [SerializeField] private float ledgeRaysSpacing;
    [SerializeField] private Vector3 ledgeRaysPositionOffset;
    private bool topLedgeRayDetected;
    private bool bottomLedgeRayDetected;

    [Header(" Rays Info ")]
    private Vector3 topRayStartPosition;
    private Vector3 topRayDestination;
    private Vector3 bottomRayStartPosition;
    private Vector3 bottomRayDestination;
    public bool IsWallDetected() => Physics2D.BoxCast(transform.position, wallCheckBoxSize, 0, new Vector2(Player.Instance.facingDirection, 0), wallCastDistance, whatIsWall);
    public bool bIsJumping = false;
    public bool IsLedgeDetected() => DetectLedges();
    public void EnableLedgeRays() => ledgeRaysEnabled = true;
    public void DisableLedgeRays() => ledgeRaysEnabled = false;
    public bool DetectLedges() 
    {
        if(ledgeRaysEnabled)
        {
            TopRayDetection();
            BottomRayDetection();
            return (!topLedgeRayDetected && bottomLedgeRayDetected);
        }
        return false;
    }
    
    private void TopRayDetection()
    {
        float xStart = transform.position.x + ledgeRaysPositionOffset.x;
        float yStart = transform.position.y + ledgeRaysPositionOffset.y + ledgeRaysSpacing;
        topRayStartPosition = new Vector3(xStart,yStart);
        topRayDestination = new Vector3(xStart + ledgeRaysDistance * Player.Instance.facingDirection, yStart);

        topLedgeRayDetected = RayCastDetectWall(topRayStartPosition, ledgeRaysDistance);
    }
    
    private void BottomRayDetection()
    {
        float xStart = transform.position.x + ledgeRaysPositionOffset.x;
        float yStart = transform.position.y + ledgeRaysPositionOffset.y - ledgeRaysSpacing;
        bottomRayStartPosition = new Vector2(xStart,yStart);
        bottomRayDestination = new Vector3(xStart + ledgeRaysDistance * Player.Instance.facingDirection, yStart);
        bottomLedgeRayDetected = RayCastDetectWall(bottomRayStartPosition, ledgeRaysDistance);
    }

    private bool RayCastDetectWall(Vector2 startPosition, float distance)
    {
        Vector3 start = new Vector3(startPosition.x ,startPosition.y, 0);
        return Physics2D.Raycast(start, Vector2.right * Player.Instance.facingDirection, distance, whatIsWall);
    }
    
    // When object in hierarchy is selected, will show where the ground and wall checks will happen
    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireCube(transform.position - transform.up * groundCastDistance, groundCheckBoxSize); // GroundCheck * Player.Instance.facingDirection
        Gizmos.DrawWireCube(transform.position - new Vector3(Player.Instance.facingDirection, 0) * wallCastDistance, wallCheckBoxSize); // WallCheck

        // Ledge Detection
        if (ledgeRaysEnabled)
        {
            Gizmos.color = topLedgeRayDetected ? Color.green : Color.grey;
            Gizmos.DrawLine(topRayStartPosition, topRayDestination);
            Gizmos.color = bottomLedgeRayDetected ? Color.green : Color.grey;
            Gizmos.DrawLine(bottomRayStartPosition, bottomRayDestination);
        }
    }
}
