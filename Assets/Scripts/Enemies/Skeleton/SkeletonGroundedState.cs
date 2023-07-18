using Assets.Scripts.Enemies;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected EnemySkeleton _enemySkeleton;
    protected Transform player;
    private short playerDetectFromBehindDistance = 2;

    public SkeletonGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName)
    {
        _enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_enemySkeleton.IsPlayerDetected() || Vector2.Distance(_enemySkeleton.transform.position, player.position) < playerDetectFromBehindDistance)
            _stateMachine.ChangeState(_enemySkeleton.SkeletonBattleState);
    }
}