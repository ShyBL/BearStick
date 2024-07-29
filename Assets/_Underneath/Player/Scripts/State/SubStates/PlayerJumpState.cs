
public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
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
        HoldAirState();
    }

    private void HoldAirState()
    {
        if (player.playerPhysx.CurrentVelocity().y < 0)
        {
            stateMachine.ChangeState(stateMachine.AirState);
        }
    }
}
