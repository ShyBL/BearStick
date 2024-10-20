public class PlayerLedgeGrabState : PlayerState
{
    public PlayerLedgeGrabState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        player.playerPhysx.SetVelocity(0, 0);
        player.playerPhysx.DisableGravity();
        isGrabbingLedge = true;
        Subscribe();

    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        player.playerPhysx.EnableGravity();
        player.StartCoroutine(StopLedgeRaysFor(.5f));
        Unsubscribe();
        isGrabbingLedge = false;
    }

    private void Subscribe()
    {
        player.playerInput.onJump += OnJump;
        player.playerInput.onMove += OnMove;
    }

    private void Unsubscribe()
    {
        player.playerInput.onJump -= OnJump;
        player.playerInput.onMove -= OnMove;
    }

    public void OnMove()
    {
        player.StartCoroutine(StopLedgeRaysFor(.5f));
        stateMachine.ChangeState(stateMachine.AirState);
    }

    private void OnJump()
    {
        stateMachine.ChangeState(stateMachine.JumpState);
    }
}
