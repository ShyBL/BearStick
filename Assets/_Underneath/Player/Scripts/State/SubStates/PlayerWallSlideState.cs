using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
    {
    }

    int _wallDirection;

    public override void Enter()
    {
        base.Enter();
        player.playerInput.onJump += OnJump;
        _wallDirection = player.facingDirection;

    }


    public override void Update()
    {
        base.Update();
        Slide();
        IsOnWall();
        LedgeDetection();
    }



    public override void Exit()
    {
        base.Exit();
        player.playerInput.onJump -= OnJump;

    }

    private void OnJump()
    {
        stateMachine.ChangeState(stateMachine.WallJumpState);
    }


    private void Slide()
    {
       player.playerPhysx.SetVelocity(moveInputVector.x, player.Velocity().y * player.wallSlideSpeed);
    }

    private void IsOnWall()
    {
        if (player.Velocity().y == 0 && !isGrabbingLedge || !player.playerPhysx.IsWallDetected())
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    
}
