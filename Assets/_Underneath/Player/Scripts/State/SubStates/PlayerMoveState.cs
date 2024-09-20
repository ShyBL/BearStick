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

    }

    public override void Exit()
    {
        
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.AudioManager.PlayEventWithValueParameters
        ( player.AudioManager.FootstepsEvent, player.gameObject.transform.position,
            "Material", 0);
        CheckIfStopped();
        CheckIfFalling();
        CheckIfSprinting();
    }

    private void CheckIfStopped()
    {
        if (moveInputVector == Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
            
    }

    private void CheckIfFalling()
    {
        if (!player.IsGrounded())
        {
            stateMachine.ChangeState(stateMachine.AirState);
        }
    }

    private void CheckIfSprinting()
    {
        if(player.isSprinting == true)
        {
             stateMachine.ChangeState(stateMachine.SprintState);
        }
    }
}