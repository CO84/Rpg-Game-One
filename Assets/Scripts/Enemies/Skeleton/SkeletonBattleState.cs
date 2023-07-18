using Assets.Scripts.Enemies;
using Assets.Scripts.Enums;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton _enemySkeleton;
    private int moveDirection;
    private int battleStateFinishDistance = 10;

    public SkeletonBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName)
    {
        _enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();

        //palyer manager oluþturmadan kullanýlan yöntem
        //player = GameObject.Find(EnemyConstants.PLAYER).transform;

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(_enemySkeleton.IsPlayerDetected())
        {
            stateTimer = _enemySkeleton.battleTime;
            if(_enemySkeleton.IsPlayerDetected().distance < _enemySkeleton.attackDistance)
            {
                if(CanAttack())
                _stateMachine.ChangeState(_enemySkeleton.SkeletonAttackState);
                return;
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, _enemySkeleton.transform.position) > battleStateFinishDistance)
                _stateMachine.ChangeState(_enemySkeleton.SkeletonIdleState);
        }

        if (player.position.x > _enemySkeleton.transform.position.x)
            moveDirection = (int)PlayerDirectionEnum.Right;
        else if(player.position.x < _enemySkeleton.transform.position.x)
            moveDirection = (int)PlayerDirectionEnum.Left;

        _enemySkeleton.SetVelocity(_enemySkeleton.moveSpeed * moveDirection, rb.velocity.y);
    }

    private bool CanAttack()
    {
        if(Time.time >= _enemySkeleton.lastTimeAttacked + _enemySkeleton.attackCooldown)
        {
            _enemySkeleton.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
