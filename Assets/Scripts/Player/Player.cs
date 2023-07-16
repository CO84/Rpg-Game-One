using System.Collections;
using UnityEngine;

public class Player : BaseEntity
{

    [Header("Attack Details")]
    public Vector2[] attackMovement;


    public bool IsBusy { get; private set; } = false;

    #region MOVE INFO
    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce;

    #endregion

    #region DASH INFO
    [Header("Dash Info")]
    [SerializeField] private float dashCooldown;
    protected float dashUsageTimer;
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
    #endregion

    #region ATTACK

    public PlayerPrimaryAttack PlayerPrimaryAttack { get; private set; }

    #endregion

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
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(PlayerIdleState);
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.Update();
        CheckForDashInput();
        

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

        dashUsageTimer =  dashUsageTimer >= 0 ? dashUsageTimer -= Time.deltaTime : dashUsageTimer;
        //dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDirection = Input.GetAxisRaw(PlayerConstants.HORIZONTAL);

            if(dashDirection is 0)
            {
                dashDirection = facingDirection;
            }

            StateMachine.ChangeState(PlayerDashState);
        }

           
    }






}
