using System;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : OurMonoBehaviour
{
    // This boolean determines if this interaction can happen or not.
    private bool _inRange;
    public Player Player;
    
    [SerializeField]
    public UnityEvent OnTriggerEnter;
    [SerializeField]
    public UnityEvent OnTriggerExit;
    [SerializeField]
    public UnityEvent OnInteract;

    public bool Interactable;

    private void Awake()
    {
        Interactable = true;
    }

    // When the player collides with this object, a boolean is set to signal that the player is in range.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (Interactable)
            {
                Player = player;

                player.playerInput.onInteract += Interact;
                _inRange = true;

                OnTriggerEnter.Invoke();
            }
        }
    }

    // When the player is no longer in contact with the object, the bool is unset.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (Interactable)
            if (other.TryGetComponent(out Player player))
            {
                player.playerInput.onInteract -= Interact;
                _inRange = false;

                OnTriggerExit.Invoke();
            }
    }

    // Allows the input action to trigger the interaction
    private void Interact()
    {
        if(!_inRange) return;
        OnInteract.Invoke();
    }
    
}