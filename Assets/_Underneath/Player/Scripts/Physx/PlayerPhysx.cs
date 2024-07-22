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

    public bool IsGrounded => Physics.BoxCast(
        playerTransform.position,
        groundCheckBoxSize / 2,
        -playerTransform.up,
        out _,
        playerTransform.rotation,
        groundCastDistance,
        whatIsGround
    );
    public void HandleMovement(Vector3 movement, float speed)
    {
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y,movement.z * speed);
    }
    
    public Vector3 CurrentVelocity() => new Vector3(rb.velocity.x, rb.velocity.y);
    

    public void Jump(Vector3 jumpVector, float airVelocity, float jumpForce)
    {
        rb.velocity = new Vector3(jumpVector.x * airVelocity, jumpForce,jumpVector.z * airVelocity);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(playerTransform.position - playerTransform.up * groundCastDistance, groundCheckBoxSize); // ground check visual
    }
}
