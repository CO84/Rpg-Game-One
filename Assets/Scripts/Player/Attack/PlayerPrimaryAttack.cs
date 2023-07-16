using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttack(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
       // xInput = 0; // fix bug on the attack direction

        if(comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        _player.animator.SetInteger(PlayerConstants.COMBOCOUNTER, comboCounter);

        //choose attack direction
        float attackDrirection = _player.facingDirection;
        if (xInput is not 0) attackDrirection = xInput;

        _player.SetVelocity(_player.attackMovement[comboCounter].x * attackDrirection, _player.attackMovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        _player.StartCoroutine(PlayerConstants.BUSYFOR, .15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            _player.SetZeroVelocity();

        if (triggerCalled)
            _stateMachine.ChangeState(_player.PlayerIdleState);
    }
}
