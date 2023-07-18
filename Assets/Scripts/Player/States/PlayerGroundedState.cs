using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Q))
            _stateMachine.ChangeState(_player.PlayerCounterAttackState);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            _stateMachine.ChangeState(_player.PlayerPrimaryAttack);

        if (!_player.IsGroundDetected())
            _stateMachine.ChangeState(_player.PlayerAirState);

        if (Input.GetKeyDown(KeyCode.Space) && _player.IsGroundDetected())
            _stateMachine.ChangeState(_player.PlayerJumpState);
    }
}
