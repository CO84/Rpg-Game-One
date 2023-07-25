using Assets.Scripts.Enums;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player.SkillManager.SwordThrowSkills.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        _player.StartCoroutine(PlayerConstants.BUSYFOR, .2f);
    }

    public override void Update()
    {
        base.Update();

        _player.SetZeroVelocity();

        if (Input.GetKeyUp(KeyCode.Mouse1))
            _stateMachine.ChangeState(_player.PlayerIdleState);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_player.transform.position.x > mousePosition.x && _player.facingDirection == (int)PlayerDirectionEnum.Right)
            _player.Flip();

        else if (_player.transform.position.x < mousePosition.x && _player.facingDirection == (int)PlayerDirectionEnum.Left)
            _player.Flip();

    }
}
