using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private EnemySkeleton _enemySkeleton;

    public SkeletonAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName)
    {
        _enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        _enemySkeleton.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        _enemySkeleton.SetZeroVelocity();

        if (_triggerCalled)
            _stateMachine.ChangeState(_enemySkeleton.SkeletonBattleState);
    }
}
