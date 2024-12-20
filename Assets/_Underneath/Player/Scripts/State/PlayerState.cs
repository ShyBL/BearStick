using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    [Header(" Components ")]
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Vector3 rbVelocity;

    [Header(" Settings ")]
    protected float stateDuration;
    private string _animName;
    protected float animLength;

    protected Vector3 moveInputVector = Vector3.zero;


    [Header(" State ")]
    protected bool canAttack = true;//POSSIBLE CUT, LEAVE FOR NOW
    protected bool isBusy = false;
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string animName)
    {
        player = _player;
        stateMachine = _stateMachine;
        this._animName = animName;
    }

    /// <summary>
    /// On Enter Into state
    /// </summary>
    public virtual void Enter()
    {
        player.playerVisualizer.PlayAnimation(_animName);
        //Debug.Log("Entered : " + this.ToString());
    }

    /// <summary>
    ///  Update for State
    /// </summary>
    public virtual void Update()
    {
        stateDuration -= Time.deltaTime;
        if (player.canMove)
            SetMovementVector();
        //player.playerVisualizer.SetYBlend(rbVelocity.y);

    }

    /// <summary>
    /// On Exit from state
    /// </summary>
    public virtual void Exit()
    {
    }

    private void SetMovementVector()
    {
        moveInputVector = player.moveInputVector;
    }


    protected IEnumerator BusyFor(float seconds)
    {
        isBusy = true;
        Debug.Log("Started");
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }
    
    public IEnumerator StopLedgeRaysFor(float _seconds)
    {
        player.playerPhysx.DisableLedgeRays();
        yield return new WaitForSeconds(_seconds);
        player.playerPhysx.EnableLedgeRays();

    }
    
    protected bool isGrabbingLedge;
    
    // protected void LedgeDetection()
    // {
    //     if (player.playerPhysx.IsLedgeDetected())
    //         stateMachine.ChangeState(stateMachine.LedgeGrabState);
    //
    // }
}
