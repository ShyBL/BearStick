using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Sprite talkingSprite;
    [SerializeField] private List<String> dialogueLines;

    public static TutorialManager Instance;
    
    public bool startLevel;
    public bool startLevelDOONCE = true;
    
    public bool jumping;
    public bool jumpingDOONCE = true;
    
    public bool pushing;
    public bool pushingDOONCE = true;
    
    public bool opening;
    public bool openingDOONCE = true;
    
    public bool carrying;
    public bool carryingDOONCE = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

    }

    
    void Update()
    {
        if (startLevel && startLevelDOONCE)
        {
            dialogue.StartDialogue(dialogueLines[0],talkingSprite);
            startLevelDOONCE = false;
        }

        if (jumping && jumpingDOONCE)
        {
            dialogue.StartDialogue(dialogueLines[1],talkingSprite);
            jumpingDOONCE = false;
        }
        
        if (pushing && pushingDOONCE)
        {
            dialogue.StartDialogue(dialogueLines[2],talkingSprite);
            pushingDOONCE = false;
        }
        
        if (opening && openingDOONCE)
        {
            dialogue.StartDialogue(dialogueLines[3],talkingSprite);
            openingDOONCE = false;
        }
        
        if (carrying && carryingDOONCE)
        {
            dialogue.StartDialogue(dialogueLines[4],talkingSprite);
            carryingDOONCE = false;
        }
        
    }
}
