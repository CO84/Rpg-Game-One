using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, _player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (rigidbody2D.velocity.y < 0)
            _stateMachine.ChangeState(_player.PlayerAirState);

        //if (_player.IsGroundDetected())
        //    _stateMachine.ChangeState(_player.PlayerIdleState);
    }
}
