using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualizer : MonoBehaviour
{
    private Animator animatorComponent;
    public SpriteRenderer spriteComponent;
    [SerializeField] private Player racc;
    
    [SerializeField] private string idleBoolParameter;
    [SerializeField] private string runBoolParameter;
    [SerializeField] private string climbingBoolParameter;
    [SerializeField] private string deathBoolParameter;
    [SerializeField] private string hurtBoolParameter;
    [SerializeField] private string jumpingBoolParameter;
    [SerializeField] private string fallingBoolParameter;
    [SerializeField] private string wallSlidingBoolParameter;
    [SerializeField] private string pushingBoolParameter;
    [SerializeField] private string pullingBoolParameter;

    private List<string> allBoolParameters;
    private string currentState = null;

    private void Awake()
    {
        animatorComponent = GetComponent<Animator>();
        spriteComponent = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        allBoolParameters = new List<string>()
        {
            idleBoolParameter,
            runBoolParameter,
            climbingBoolParameter,
            deathBoolParameter,
            hurtBoolParameter,
            jumpingBoolParameter,
            fallingBoolParameter,
            wallSlidingBoolParameter,
            pushingBoolParameter,
            pullingBoolParameter
        };
    }

    private void Update()
    {
        if (racc.isGrounded())
        {
            animatorComponent.SetBool("grounded", true);
        }
        else
        {
            animatorComponent.SetBool("grounded", false);
        }
    }

    public void SetIdle() => SetBoolState(idleBoolParameter);
    public void SetRun() => SetBoolState(runBoolParameter);
    public void SetClimbing() => SetBoolState(climbingBoolParameter);
    public void SetDeath() => SetBoolState(deathBoolParameter);
    public void SetHurt() => SetBoolState(hurtBoolParameter);
    public void SetJump() => SetBoolState(jumpingBoolParameter);
    public void SetFall() => SetBoolState(fallingBoolParameter);
    public void SetWallSlide() => SetBoolState(wallSlidingBoolParameter);
    public void SetPush() => SetBoolState(pushingBoolParameter);
    public void SetPull() => SetBoolState(pullingBoolParameter);

    private void SetBoolState(string stateToEnable)
    {
        if (stateToEnable == currentState) return;
        
        foreach (var boolParameter in allBoolParameters)
        {
            animatorComponent.SetBool(boolParameter, boolParameter == stateToEnable);
        }
        currentState = stateToEnable;
    }
}