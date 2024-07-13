using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class goalArea : MonoBehaviour
{
    // Simple event that sends the list of pickups on this goal area to the listener.
    public event EventHandler<List<pickup>> GoalAreaContact;

    // Here is a list of pickups, serialized so level designers can add pickups to the list for the goal area.
    [SerializeField] private List<pickup> pickupList;

    // This boolean determines if this goal area has been used or not.
    private bool triggered = false;

    // The player needs a "Player" tag. When the player collides with this object, so long as triggered is false, the event is triggered and the boolean is set.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            Debug.Log("You have contacted the Dumpster! and gotten its items.");
            GoalAreaContact?.Invoke(this, pickupList);
            triggered = true;
        }
    }
}