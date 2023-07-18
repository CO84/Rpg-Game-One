using System.Diagnostics;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName, enemySkeleton)
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
        
        _enemySkeleton.SetVelocity(_enemySkeleton.moveSpeed * _enemySkeleton.facingDirection, rb.velocity.y);
        
        if(_enemySkeleton.IsWallDetected() || !_enemySkeleton.IsGroundDetected())
        {
            _enemySkeleton.Flip();
            _stateMachine.ChangeState(_enemySkeleton.SkeletonIdleState);
        }
    }
}
