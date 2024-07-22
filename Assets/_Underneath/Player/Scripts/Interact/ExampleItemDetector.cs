using UnityEngine;

public class ExampleDetector : MonoBehaviour
{
    public GameObject detectedItem;
    public bool itemDetected;
    public void ResetItemDetector()
    {
        detectedItem = null;
        itemDetected = false;
    }
}
public class ExampleItemDetector : ExampleDetector
{
    private void OnTriggerEnter(Collider other)
    {
        // replace PhysicsCollectible class with any other feature
        if (other.TryGetComponent(out PhysicsCollectible collectible)) 
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