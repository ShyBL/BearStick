using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAirState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.DisableMovement();
        player.wallJumpCounter++;
        player.playerPhysx.bIsJumping = true;
        player.playerPhysx.SetVelocity(player.wallJumpForce.x * -player.facingDirection, player.wallJumpForce.y);
        player.playerVisualizer.spriteComponent.flipX = !player.playerVisualizer.spriteComponent.flipX;
        //player.DisableDash();
    }

    public override void Update()
    {
        base.Update();
        if (player.IsGrounded())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        HoldAirState();
    }

    private void HoldAirState()
    {
        if (player.playerPhysx.CurrentVelocity().y < 0)
        {
            player.playerPhysx.bIsJumping = false;
            player.EnableMovement();
            stateMachine.ChangeState(stateMachine.AirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.EnableMovement(); 
        player.playerPhysx.bIsJumping = false;
        //player.EnableDash();

    }
}
