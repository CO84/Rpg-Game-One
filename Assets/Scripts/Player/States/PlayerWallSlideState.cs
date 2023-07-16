using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.ChangeState(_player.PlayerWallJumpState);
            return;
        }


        //TODO player bug var idle state olmayacak
        //if (xInput != 0 && _player.facingDirection != xInput && _player.IsGroundDetected())
        //    _stateMachine.ChangeState(_player.PlayerIdleState);

        if (yInput < 0)
            _player.ChangeVelocity(0, rigidbody2D.velocity.y);
        else
            _player.ChangeVelocity(0, rigidbody2D.velocity.y * .7f);

        if (_player.IsGroundDetected())
            _stateMachine.ChangeState(_player.PlayerIdleState);

    }
}
