using System;
using System.Collections.Generic;
using UnityEngine;

public class Npc : InteractiveObject
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] protected List<String> dialogueLines;
    [SerializeField] private Sprite talkingSprite;

    protected void DoInteraction()
    {
        foreach (var line in dialogueLines)
        {
            dialogue.StartDialogue(line,talkingSprite);
        }
    }
    
    protected void DoGossip()
    {
        foreach (var line in dialogueLines)
        {
            dialogue.StartGossip(line, transform.position);
        }
    }
}