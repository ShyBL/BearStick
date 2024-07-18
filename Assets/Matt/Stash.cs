using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Stash : MonoBehaviour
{
    // Here is a list of pickups, serialized so level designers can add pickups to the list for the goal area.
    [SerializeField] private List<pickup> pickupList;
    [SerializeField] private PlayerInput playerInput;

    // This boolean determines if this goal area has been used or not.
    private bool inRange = false;

    // The player needs a "Player" tag. When the player collides with this object, so long as triggered is false, the event is triggered and the boolean is set.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    private void OnEnable()
    {
        playerInput.onInteract += Interact;
    }

    private void Interact()
    {
        if (inRange)
        {

        }
    }
}