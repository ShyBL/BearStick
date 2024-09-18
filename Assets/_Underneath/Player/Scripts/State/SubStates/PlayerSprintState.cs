
public class PlayerSprintState : PlayerGroundedState
{
    public PlayerSprintState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
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
          
          AudioManager.Instance.PlayEventWithValueParameters
        (AudioManager.Instance.FootstepsEvent, player.gameObject.transform.position,
            "Material", 0);

        HoldSprintState();
    }

    private void HoldSprintState()
    {
        if(!player.isSprinting)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
    }
}
