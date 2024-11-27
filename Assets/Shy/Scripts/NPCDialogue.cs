using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCDialogue : OurMonoBehaviour
{
    private Dialogue dialogue;
    [SerializeField] protected List<String> dialogueLines;
    [SerializeField] private Sprite talkingSprite;

    private void Awake()
    {
        dialogue = FindFirstObjectByType<Dialogue>();
    }

    // TODO Temp for tutorial demo
    public void DoDialogue()
    {
        if (TutorialManager.Instance.startLevel == false)
        {
            TutorialManager.Instance.startLevel = true; 
        }
        
    }
    
    // public void DoDialogue()
    // {
    //     if (dialogueLines.Count == 0) return;
    //
    //     foreach (var line in dialogueLines.Where(line => line != string.Empty))
    //     {
    //         dialogue.StartDialogue(line,talkingSprite);
    //     }
    // }
    
    public void DoGossip()
    {
        foreach (var line in dialogueLines)
        {
            //dialogue.StartGossip(line, transform.position);
        }
    }
}