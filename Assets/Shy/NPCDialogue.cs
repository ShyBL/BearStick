using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCDialogue : OurMonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] protected List<String> dialogueLines;
    [SerializeField] private Sprite talkingSprite;

    public void DoDialogue()
    {
        if (dialogueLines.Count == 0) return;

        foreach (var line in dialogueLines.Where(line => line != string.Empty))
        {
            dialogue.StartDialogue(line,talkingSprite);
        }
    }
    
    public void DoGossip()
    {
        foreach (var line in dialogueLines)
        {
            //dialogue.StartGossip(line, transform.position);
        }
    }
}