using Assets.Scripts;
using UnityEngine;

public class EnemyState : IStateMachine
{
    protected EnemyStateMachine _stateMachine;
    protected Enemy _enemy;
    protected Rigidbody2D rb;

    protected bool _triggerCalled;
    private string _animBoolName;
    protected float stateTimer;

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
        _animBoolName = animBoolName;
    }

    public virtual void Enter() 
    {
        _triggerCalled = false;
        rb = _enemy.rigidbody2D;
        _enemy.animator.SetBool(_animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        _enemy.animator.SetBool(_animBoolName, false);
    }

    public virtual void AnimationFinishTrigger() 
    {
        _triggerCalled = true; 
    }
}
