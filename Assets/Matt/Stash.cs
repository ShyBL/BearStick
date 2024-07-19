using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stash : MonoBehaviour
{
    // Here is a list of pickups, serialized so level designers can add pickups to the list for the goal area.
    [SerializeField] private List<Collectable> collectableList;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private GameObject physicsCollectable;

    // This boolean determines if this goal area has been used or not.
    private bool inRange = false;

    // The player needs a "Player" tag. When the player collides with this object, a boolean is set to signal that the player is in range.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
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

    // Subscribes this object to the player's input action.
    private void OnEnable()
    {
        playerInput.onInteract += Interact;
    }

    // Allows the input action to spawn collectibles from the list as long as the player is within range.
    private void Interact()
    {
        if (inRange)
        {
            foreach (Collectable collectable in collectableList)
            {
                PhysicsCollectible newCollectable = Instantiate(physicsCollectable).GetComponent<PhysicsCollectible>();
                newCollectable.SetCollectable(collectable);
            }
        }
    }
}