public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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

        if (_player.IsWallDetected())
            _stateMachine.ChangeState(_player.PlayerWallSlideState);

        if(_player.IsGroundDetected())
        {
            _stateMachine.ChangeState(_player.PlayerIdleState);
        }

        if (xInput is not 0)
            _player.SetVelocity(_player.moveSpeed * .8f * xInput, rigidbody2D.velocity.y);
    }
}
