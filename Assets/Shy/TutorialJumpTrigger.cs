using UnityEngine;

public class TutorialJumpTrigger : MonoBehaviour
{

    private bool inRange = false;
    private bool doOnce = true;

    // The player needs a "Player" tag. When the player collides with this object, a boolean is set to signal that the player is in range.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            TutorialTriggered();
        }
    }

    // When the player no longer is in contact with the object, the bool is unset.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
    
    
    // Allows the input action to spawn collectibles from the list as long as the player is within range.
    private void TutorialTriggered()
    {
        if (inRange && doOnce)
        {
            Player.Instance.DisableMovement(); // Example of using Player capabilities, make sure the player is not moving while interacting

            TutorialManager.Instance.jumping = true;
            doOnce = false;
            
            Player.Instance.EnableMovement();
        }
    }
}