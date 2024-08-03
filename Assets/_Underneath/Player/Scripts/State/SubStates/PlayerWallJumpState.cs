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
        player.Flip();
        player.playerPhysx.SetVelocity(player.wallJumpForce * -player.facingDirection, player.jumpForce);
        player.playerPhysx.bIsJumping = true;
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
            stateMachine.ChangeState(stateMachine.AirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.playerPhysx.bIsJumping = false;
        //player.EnableDash();

    }
}
