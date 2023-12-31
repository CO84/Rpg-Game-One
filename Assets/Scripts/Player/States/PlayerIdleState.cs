using Assets.Scripts.Enums;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //TODO
        //player flicking move and idle with if x input == facedirection
        //if (xInput == _player.facingDirection && _player.IsWallDetected())
        //    return;


        if (_player.IsWallDetected())
        {
            if (xInput > 0 && _player.facingDirection == (int)PlayerDirectionEnum.Right) return;

            else if (xInput < 0 && _player.facingDirection == (int)PlayerDirectionEnum.Left) return;
        }

        if (xInput is not 0 && !_player.IsBusy)
            _stateMachine.ChangeState(_player.PlayerMoveState);
    }
}
