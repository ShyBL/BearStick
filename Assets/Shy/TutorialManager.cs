using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Sprite talkingSprite;
    [SerializeField] private List<String> dialogueLines;

    [SerializeField] private float levelTimer;
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

    //TESTING
    public bool bIsSavingGame = false;
    public bool bIsLoadingGame = false;

    public bool bPauseGame = false;
    public bool bResumeGame = false;
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

    private void Start()
    {
       SavingAndLoading.Instance.CheckIfFileExistsOnStart();

        //Start the timer with whatever the level's timer will be
        CurfewTimer.Instance.StartTimer(levelTimer);

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
        
        //Testing Stuff
        if(bIsLoadingGame)
        {
            SavingAndLoading.Instance.LoadPlayerInformation();
            bIsLoadingGame = false;
        }

        if(bIsSavingGame)
        {
            SavingAndLoading.Instance.SavePlayerInformation();
            bIsSavingGame = false;

        }

        if(bPauseGame)
        {
            CurfewTimer.Instance.PauseTimer();
            bPauseGame = false;
        }

        if (bResumeGame)
        {
            CurfewTimer.Instance.ResumeTimer();
            bResumeGame = false;
        }
    }
}
