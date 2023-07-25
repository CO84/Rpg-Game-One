using Assets.Scripts.Enums;
using System.Transactions;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{

    private Transform sword;

    public PlayerCatchSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = _player.Sword.transform;

        if (_player.transform.position.x > sword.position.x && _player.facingDirection == (int)PlayerDirectionEnum.Right)
            _player.Flip();

        else if (_player.transform.position.x < sword.position.x && _player.facingDirection == (int)PlayerDirectionEnum.Left)
            _player.Flip();

        _player.ChangeVelocity(_player.swordReturnImpact * -_player.facingDirection, rigidbody2D.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        _player.StartCoroutine(PlayerConstants.BUSYFOR, .1f);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            _stateMachine.ChangeState(_player.PlayerIdleState);
    }
}
