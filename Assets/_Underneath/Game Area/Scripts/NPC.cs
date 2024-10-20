using System;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool inRange = false;
    private bool tutorialDoOnce = true;
    
    private void Start()
    {
        _animator.Play("Idle");
        Player.Instance.playerInput.onInteract += Interact;
    }
    
    // The player needs a "Player" tag. When the player collides with this object, a boolean is set to signal that the player is in range.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            _animator.Play("Talking");
        }
        
        if (inRange && tutorialDoOnce)
        {
            Player.Instance.DisableMovement(); 

            TutorialManager.Instance.startLevel = true;
            tutorialDoOnce = false;
            
            Player.Instance.EnableMovement();
        }
    }

    // When the player no longer is in contact with the object, the bool is unset.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            _animator.Play("Idle");
            CurfewTimer.Instance.ResumeTimer();
        }
    }
    
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private List<String> dialogueLines;
    [SerializeField] private Sprite talkingSprite;

    private void Interact()
    {
        if (inRange && !tutorialDoOnce && dialogueLines.Count != 0)
        {
            dialogue.StartDialogue(dialogueLines[0],talkingSprite);
            CurfewTimer.Instance.PauseTimer();
        }
    }
}