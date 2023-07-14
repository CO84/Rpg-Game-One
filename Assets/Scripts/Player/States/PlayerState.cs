using UnityEngine;

public class PlayerState 
{
    protected PlayerStateMachine _stateMachine;
    protected Player _player;
    protected Rigidbody2D rigidbody2D;

    protected float xInput;
    private readonly string _animBoolName;

    protected float stateTimer;
    protected float yInput;
    protected bool triggerCalled;

    public PlayerState(PlayerStateMachine stateMachine, Player player, string animBoolName)
    {
        _stateMachine = stateMachine;
        _player = player;
        _animBoolName = animBoolName;
    }

    public virtual void Enter() 
    {
        _player.animator.SetBool(_animBoolName, true);
        rigidbody2D = _player.rigidbody2D;
        triggerCalled = false;
    }
    public virtual void Update() 
    {
        stateTimer -= Time.deltaTime;
        
        xInput = Input.GetAxis(PlayerConstants.HORIZONTAL);
        yInput = Input.GetAxis(PlayerConstants.VERTICAL);

        _player.animator.SetFloat(PlayerConstants.YVELOCITY, rigidbody2D.velocity.y);
    }
    public virtual void Exit()
    {
        _player.animator.SetBool(_animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    } 
}
