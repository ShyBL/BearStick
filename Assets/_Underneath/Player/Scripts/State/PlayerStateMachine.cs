using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }
    private Player _player;

    #region [--- States ---]
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerSprintState SprintState {get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    //public PlayerLedgeGrabState LedgeGrabState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    //public PlayerWallSlideState WallSlideState { get; private set; }

    //public PlayerPickUpState PickUpState { get; private set; }
    

    #endregion [--- States ---]
    public void Initialize()
    {
        InitPlayer();
        CreateStates();
        currentState = IdleState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState state)
    { 
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
    public void CreateStates()
    {
        IdleState = new PlayerIdleState(_player, this, States.IDLE);
        MoveState = new PlayerMoveState(_player, this, States.MOVE);
        SprintState = new PlayerSprintState(_player, this, States.SPRINT);
        AirState = new PlayerAirState(_player, this, States.AIR);
        JumpState = new PlayerJumpState(_player,this, States.JUMP);
        
        //LedgeGrabState = new PlayerLedgeGrabState(_player,this, States.SLIDE);
        WallJumpState = new PlayerWallJumpState(_player,this, States.JUMP);
        //WallSlideState = new PlayerWallSlideState(_player,this, States.SLIDE);

        //PickUpState = new PlayerPickUpState(_player, this, States.PICKUP);

    }



    private void InitPlayer()
    {
        _player = Player.Instance;
    }

}