using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NPC : MonoBehaviour
{

    [SerializeField] 
    private GameObject textGameObject;

    private bool inRange = false;
    private bool doOnce = true;

    // The player needs a "Player" tag. When the player collides with this object, a boolean is set to signal that the player is in range.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            //textGameObject.SetActive(true);
        }
        
        if (inRange && doOnce)
        {
            Player.Instance.DisableMovement(); // Example of using Player capabilities, make sure the player is not moving while interacting

            TutorialManager.Instance.startLevel = true;
            doOnce = false;
            
            Player.Instance.EnableMovement();
        }
    }

    // When the player no longer is in contact with the object, the bool is unset.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            //textGameObject.SetActive(false);
        }
    }

    // Subscribes this object to the player's input action.
    private void Start()
    {
        //Player.Instance.playerInput.onInteract += Interact;
    }
    
    // Allows the input action to spawn collectibles from the list as long as the player is within range.
    private void Interact()
    {
        if (inRange && doOnce)
        {
            Player.Instance.DisableMovement(); // Example of using Player capabilities, make sure the player is not moving while interacting

            TutorialManager.Instance.startLevel = true;
            doOnce = false;
            
            Player.Instance.EnableMovement();
        }
    }
}