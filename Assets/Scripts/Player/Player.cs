using UnityEngine;

public class Player : MonoBehaviour
{
    #region MOVE INFO

    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce;

    #endregion

    #region COLLISION INFO

    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    #endregion

    #region DASH INFO
    [Header("Dash Info")]
    [SerializeField] private float dashCooldown;
    protected float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection { get; private set; }
    #endregion

    public int facingDirection { get; private set; } = 1;
    private bool facingRight = true;

    #region Components
    public Animator animator {  get; private set; }
    public new Rigidbody2D rigidbody2D { get; private set; }
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

    private void Awake()
    {
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

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        StateMachine.Initialize(PlayerIdleState);
    }

    private void Update()
    {
        StateMachine.currentState.Update();
        CheckForDashInput();

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

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rigidbody2D.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance,whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
    }

    public void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float x)
    {
        if(x > 0  && ! facingRight) 
            Flip();
        else if (x < 0 && facingRight)
            Flip();
    }
}
