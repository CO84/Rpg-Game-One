

using Assets.Scripts.Enemies;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    #region STATES

    public SkeletonIdleState SkeletonIdleState { get; private set; }
    public SkeletonMoveState SkeletonMoveState { get; private set; }
    public SkeletonBattleState SkeletonBattleState { get; private set; }
    public SkeletonAttackState SkeletonAttackState { get; private set; }
    public SkeletonStunnedState SkeletonStunnedState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        SkeletonIdleState = new SkeletonIdleState(this, StateMachine, EnemyConstants.IDLE, this);
        SkeletonMoveState = new SkeletonMoveState(this, StateMachine, EnemyConstants.MOVE, this);
        SkeletonBattleState = new SkeletonBattleState(this, StateMachine, EnemyConstants.MOVE, this);
        SkeletonAttackState = new SkeletonAttackState(this, StateMachine, EnemyConstants.ATTACK, this);
        SkeletonStunnedState = new SkeletonStunnedState(this, StateMachine, EnemyConstants.STUNNED, this);
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(SkeletonIdleState);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.U))
            StateMachine.ChangeState(SkeletonStunnedState);
    }

    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
        {
            StateMachine.ChangeState(SkeletonStunnedState);
            return true;
        }
        return false;
    }
}
