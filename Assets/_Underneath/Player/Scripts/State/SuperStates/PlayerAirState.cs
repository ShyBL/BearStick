using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
    {
    }
    // Used because map generator uses Vector2 instead Vector2Int, so its not pixel perfect.
    private static float LANDING_THRESHOLD = 0.001f;

    public override void Enter()
    {
        base.Enter();
        player.playerInput.onJump += CheckIfCloseToWall;
    }

    public override void Update()
    {
        base.Update();
        //SetYBlendAnimation();
        CheckIfIdle();
    }

    private void SetYBlendAnimation()
    {
        player.playerVisualizer.SetYBlend(player.playerPhysx.CurrentVelocity().y);
    }

    public override void Exit()
    {
        base.Exit();
        player.playerInput.onJump -= CheckIfCloseToWall;
    }
    
    private void CheckIfIdle()
    {
        if (player.IsGrounded() && player.playerPhysx.CurrentVelocity().y < LANDING_THRESHOLD)
        {
            
            stateMachine.ChangeState(stateMachine.IdleState);
            player.playerPhysx.bIsJumping = false;
        }
    }

    private void CheckIfCloseToWall()
    {
        //Player detects a wall while theyre in the air
        if(player.playerPhysx.IsWallDetected() & !player.playerPhysx.bIsJumping)
        {
            stateMachine.ChangeState(stateMachine.WallJumpState);

        }
    }
}
