using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public GameObject detectedItem;
    public bool itemDetected;
    public void ResetItemDetector()
    {
        detectedItem = null;
        itemDetected = false;
    }
}
public class PlayerItemDetector : PlayerDetector
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PhysicsCollectible collectible)) // replace PhysicsCollectible class with any other feature
        {
            detectedItem = other.gameObject;
            itemDetected = true;
            
            // extra logic according to the feature
            // call any feature on the Player, and use its functions
            // or create a designated method to call the feature from within Player
            // ie MovementHandler()
            if (Player.Instance.Inventory.AddItem(collectible.collectable))
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PhysicsCollectible collectible))
        {
            ResetItemDetector();
        }
    }
}