using Assets.Scripts.Enemy;

public class SkeletonStunnedState : EnemyState
{
    private EnemySkeleton _enemySkeleton;

    public SkeletonStunnedState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName)
    {
        _enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();

        _enemySkeleton.Fx.InvokeRepeating(EnemyConstants.RED_COLOR_BLINK, 0, .1f);

        stateTimer = 1;

        _enemySkeleton.ChangeVelocity(-_enemySkeleton.facingDirection * _enemySkeleton.stunDireciton.x, _enemySkeleton.stunDireciton.y);
    }

    public override void Exit()
    {
        base.Exit();

        _enemySkeleton.Fx.Invoke(EnemyConstants.CANCEL_RED_BLINK, 0);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            _stateMachine.ChangeState(_enemySkeleton.SkeletonIdleState);
    }
}
