using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlayEvent(FMODEvents.instance.FootstepsEvent, player.gameObject.transform.position);
    }

    public override void Exit()
    {
        //AudioManager.instance.StopEvent(FMODEvents.instance.FootstepsEvent);
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        CheckIfStopped();
        CheckIfFalling();
    }

    private void CheckIfStopped()
    {
        if (moveInputVector == Vector3.zero)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void CheckIfFalling()
    {
        if (!player.IsGrounded())
            stateMachine.ChangeState(stateMachine.AirState);
    }
}