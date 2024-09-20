
public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.AudioManager.PlayEventWithValueParameters
        ( player.AudioManager.JumpEvent, player.gameObject.transform.position,
            "Material", 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
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
}
