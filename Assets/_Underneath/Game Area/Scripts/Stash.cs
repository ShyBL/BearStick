using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stash : MonoBehaviour
{
    // Here is a list of pickups, serialized so level designers can add pickups to the list for the goal area.
    [SerializeField] 
    private List<Item> collectableList;
    [SerializeField] 
    private GameObject physicsCollectable;
    [SerializeField] 
    private GameObject textGameObject;
    [SerializeField]
    private Animator _animator;

    // This boolean determines if this goal area has been used or not.
    private bool inRange = false;

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
    }

    [SerializeField] private Transform OutPoint;
    
    // Allows the input action to spawn collectibles from the list as long as the player is within range.
    private void Interact()
    {
        if (inRange)
        {
            _animator.Play("Open");
            
            Player.Instance.DisableMovement(); // Example of using Player capabilities, make sure the player is not moving while interacting
            
            foreach (Item collectable in collectableList)
            {
                PhysicsCollectible newCollectable = Instantiate(physicsCollectable,OutPoint.position,quaternion.identity).GetComponent<PhysicsCollectible>();
                newCollectable.SetCollectable(collectable);
            }
            
            Player.Instance.EnableMovement();
        }
    }
}