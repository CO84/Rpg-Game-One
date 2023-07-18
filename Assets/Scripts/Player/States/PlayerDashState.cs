public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

       _player.SkillManager.CloneSkill.CreateClone(_player.transform);

        stateTimer = _player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        _player.SetVelocity(0, rigidbody2D.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (!_player.IsGroundDetected() && _player.IsWallDetected())
            _stateMachine.ChangeState(_player.PlayerWallSlideState);

        _player.SetVelocity(_player.dashSpeed * _player.dashDirection, 0);

        if (stateTimer < 0)
            _stateMachine.ChangeState(_player.PlayerIdleState);
    }
}
