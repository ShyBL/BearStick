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
        //player.DisableDash();
    }

    public override void Update()
    {
        base.Update();
        if (player.IsGrounded())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        //player.EnableDash();

    }
}
