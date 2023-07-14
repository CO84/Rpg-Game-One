public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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

        _player.SetVelocity(xInput * _player.moveSpeed, rigidbody2D.velocity.y);

        if (xInput is 0 || _player.IsWallDetected())
            _stateMachine.ChangeState(_player.PlayerIdleState);
    }
}
