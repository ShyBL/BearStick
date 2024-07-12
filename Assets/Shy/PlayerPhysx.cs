using UnityEngine;

public class PlayerPhysx : MonoBehaviour
{
    [Header(" RB Settings ")]
    [SerializeField] private Rigidbody2D rb;
    
    public void HandleMovement(Vector3 movement, float speed)
    {
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y,movement.z * speed);
    }
}
