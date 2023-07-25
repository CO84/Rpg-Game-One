using System.Collections;
using UnityEngine;

public class Enemy : BaseEntity
{

    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    [Header("Stun Info")]
    public float stunDuration;
    public Vector2 stunDireciton;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;



    [SerializeField]protected LayerMask whatIsPlayer;
    private float playerDetectDistance = 50f;

    public EnemyStateMachine StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();  
        StateMachine.CurrentState.Update();
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if(_timeFrozen)
        {
            moveSpeed = 0;
            animator.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            animator.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region COUNTER ATTACK WINDOW
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned=false;
        counterImage.SetActive(false);
    }

    #endregion

    public virtual bool CanBeStunned()
    {
        if(canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public virtual void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerDetectDistance, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }
}
