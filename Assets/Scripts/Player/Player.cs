using Assets.Scripts.Players.States;
using System.Collections;
using UnityEngine;

public class Player : BaseEntity
{

    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    public float swordReturnImpact = 5;
    public bool IsBusy { get; private set; } = false;

    #region MOVE INFO
    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce;

    #endregion

    #region DASH INFO
    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection { get; private set; }
    #endregion

    #region STATES

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState PlayerIdleState { get; private set; }
    public PlayerMoveState PlayerMoveState { get; private set; }
    public PlayerJumpState PlayerJumpState { get; private set; }
    public PlayerAirState PlayerAirState { get; private set; }
    public PlayerDashState PlayerDashState { get; private set; }
    public PlayerWallSlideState PlayerWallSlideState { get; private set; }
    public PlayerWallJumpState PlayerWallJumpState { get; private set; }
    public PlayerCounterAttackState PlayerCounterAttackState { get; private set; }
    public PlayerAimSwordState PlayerAimSwordState { get; private set; }
    public PlayerCatchSwordState PlayerCatchSwordState { get; private set; }
    #endregion

    #region ATTACK

    public PlayerPrimaryAttack PlayerPrimaryAttack { get; private set; }

    #endregion

    public SkillManager SkillManager { get; private set; }
    public GameObject Sword { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();

        PlayerIdleState = new PlayerIdleState(StateMachine, this, PlayerConstants.IDLE);
        PlayerMoveState = new PlayerMoveState(StateMachine, this, PlayerConstants.MOVE);
        PlayerJumpState = new PlayerJumpState(StateMachine, this, PlayerConstants.JUMP);
        PlayerAirState = new PlayerAirState(StateMachine, this, PlayerConstants.JUMP);
        PlayerDashState = new PlayerDashState(StateMachine, this, PlayerConstants.DASH);
        PlayerWallSlideState = new PlayerWallSlideState(StateMachine, this, PlayerConstants.WALLSLIDE);
        PlayerWallJumpState = new PlayerWallJumpState(StateMachine, this, PlayerConstants.JUMP);
        PlayerPrimaryAttack = new PlayerPrimaryAttack(StateMachine, this, PlayerConstants.PRIMARYATTACK);
        PlayerCounterAttackState = new PlayerCounterAttackState(StateMachine, this, PlayerConstants.COUNTER_ATTACK);

        PlayerAimSwordState = new PlayerAimSwordState(StateMachine, this, PlayerConstants.AIM_SWORD);
        PlayerCatchSwordState = new PlayerCatchSwordState(StateMachine, this, PlayerConstants.CATCH_SWORD);
    }

    protected override void Start()
    {
        base.Start();

        SkillManager = SkillManager.instance;

        StateMachine.Initialize(PlayerIdleState);
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.Update();
        CheckForDashInput();

    }

    public void AssignNewSword(GameObject _newSword)
    {
        Sword = _newSword;
    }

    public void CatchTheSword()
    {
        StateMachine.ChangeState(PlayerCatchSwordState);
        Destroy(Sword);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        IsBusy = true;
        yield return new WaitForSeconds(_seconds);

        IsBusy = false;
    }

    public void AnimationTrigger() => StateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.DashSkill.CanUseSkill())
        {
            dashDirection = Input.GetAxisRaw(PlayerConstants.HORIZONTAL);

            if(dashDirection is 0)
            {
                dashDirection = facingDirection;
            }

            StateMachine.ChangeState(PlayerDashState);
        }

           
    }






}
