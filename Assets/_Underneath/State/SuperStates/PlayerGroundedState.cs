using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Subscribe();
    }

    public override void Exit()
    {
        base.Exit();
        Unsubscribe();
    }

    public override void Update()
    {
        base.Update();
    }

    private void Subscribe()
    {
        player.playerInput.onJump += OnJump;
    }



    private void Unsubscribe()
    {
        player.playerInput.onJump -= OnJump;

    }  

    private void OnJump()
    {
        if (player.isGrounded())
            stateMachine.ChangeState(stateMachine.JumpState);
    }
}
