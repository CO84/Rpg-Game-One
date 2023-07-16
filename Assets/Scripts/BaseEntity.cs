using Assets.Scripts.Contants;
using System.Collections;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    public EntityFX Fx { get; private set; }

    #region COLLISION INFO

    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public Transform attackCheck;
    public float attackCheckRadius;

    #endregion

    #region Components
    public Animator animator { get; private set; }
    public new Rigidbody2D rigidbody2D { get; private set; }
    #endregion

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    public int facingDirection { get; private set; } = 1;
    protected bool facingRight = true;


    #region AWAKE-START-UPDATE
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        Fx = GetComponent<EntityFX>();
        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }
    #endregion

    public virtual void Damage()
    {
        Fx.StartCoroutine(EntityContstant.FLASHFX);
        StartCoroutine(EntityContstant.HITKNOCKBACK);

        Debug.Log(gameObject.name + "was damaged");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        rigidbody2D.velocity = new Vector2(knockbackDirection.x * -facingDirection, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }


    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);


    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    #region VELOCITY
    public void SetZeroVelocity()
    {
        if (isKnocked) return;

        rigidbody2D.velocity = new Vector2(0, 0);
    } 
    public void ChangeVelocity(float x, float y)
    {
        rigidbody2D.velocity = new Vector2(x, y);
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked) return;
        rigidbody2D.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
    #endregion

    #region FLIP

    public virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float x)
    {
        if (x > 0 && !facingRight)
            Flip();
        else if (x < 0 && facingRight)
            Flip();
    }
    #endregion
}
