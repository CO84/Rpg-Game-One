using System.Diagnostics;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName, enemySkeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = _enemySkeleton.idleTime;
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            _stateMachine.ChangeState(_enemySkeleton.SkeletonMoveState);
    }
}
