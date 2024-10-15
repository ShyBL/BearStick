using UnityEngine;

public class InteractiveNPC : Npc
{
    private void Start()
    {
        DrawLineRenderer();
    }

    // When the player collides with this object, a boolean is set to signal that the player is in range.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            base.Player = player;
            player.playerInput.onInteract += Interact;
            InRange = true;
            BeginFocus();
        }
    }

    // When the player is no longer in contact with the object, the bool is unset.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.playerInput.onInteract -= Interact;
            InRange = false;
            EndFocus();
        }
    }

    // Allows the input action to trigger the interaction
    private void Interact()
    {
        if (!InRange || dialogueLines.Count == 0) return;
        
        PlayAnimation(animName);
        
        PlaySoundByType();

        DoInteraction();
    }
}