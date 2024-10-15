using UnityEngine;

public class NonInteractiveNPC : Npc
{

    // When the player collides with this object, a boolean is set to signal that the player is in range.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Interact();
            InRange = true;
        }
    }

    // When the player is no longer in contact with the object, the bool is unset.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            InRange = false;
        }
    }

    private void Interact()
    {
        if (!InRange || dialogueLines.Count == 0) return;
        
        PlayAnimation(animName);
        
        PlaySoundByType();
        
        DoGossip();
    }
}