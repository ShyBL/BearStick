using UnityEngine;

public class ShopControl : OurMonoBehaviour
{
    private Shop shop;
    
    [SerializeField] 
    private GameObject textGameObject;
    [SerializeField]
    private Animator _animator;
    
    // This boolean determines if this goal area has been used or not.
    private bool inRange = false;
    
    // can be a cache if you want instead of enter shop
    public bool isCache = false;

    // The player needs a "Player" tag. When the player collides with this object, a boolean is set to signal that the player is in range.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            textGameObject.SetActive(true);
        }
    }
    
    // When the player no longer is in contact with the object, the bool is unset.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            textGameObject.SetActive(false);
        }
    }
    
    // Subscribes this object to the player's input action.
    private void Start()
    {
        Player.Instance.playerInput.onInteract += Interact;
        shop = FindFirstObjectByType<Shop>();
    }
    
        
    private void Interact()
    {
        if (inRange)
        {
            Player.Instance.DisableMovement(); // Example of using Player capabilities, make sure the player is not moving while interacting
            Player.Instance.StopInPlace();
            shop.OpenShop();
        }
    }
}