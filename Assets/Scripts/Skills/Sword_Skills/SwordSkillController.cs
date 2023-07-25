using Assets.Scripts.Contants;
using Assets.Scripts.Skills;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
     private float returnSpeed = 12;

    private CircleCollider2D circleCollider;
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private Player player;

    private short swordHideDistance = 1;
    private bool isReturning;
    private bool canRotate = true;

    private float freezeTimeDuration;

    [Header("Pierce Info")]
    private float pierceAmount;

    [Header("Bounce Info")]
    private float bounceSpeed = 20;
    private readonly float bounceDetectRadius = 10;
    private List<Transform> enemyTarget;
    private short bounceAmount;
    private bool isBouncing;
    private int targetIndex;


    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinnnig;

    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        if (canRotate)
            transform.right = rigidbody.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < swordHideDistance)
            {
                player.CatchTheSword();
                animator.SetBool(EntityContstant.ROTATION, false);
            }
        }

        BounceLogic();
        SpinLogic();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetUpSword(Vector2 _direction, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        rigidbody.velocity = _direction;
        rigidbody.gravityScale = _gravityScale;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        animator.SetBool(EntityContstant.ROTATION, true);

        spinDirection = Mathf.Clamp(rigidbody.velocity.x, -1, 1);

        Invoke(EntityContstant.DESTROYME, 5);
    }
    public void ReturnSword()
    {
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        //rigidbody.isKinematic = false;
        transform.parent = null;
        isReturning = true;
        animator.SetBool(EntityContstant.ROTATION, true);
    }


    #region BOUNCE - SPIN - PIERCE SETUPS
    public void SetUpBounce(bool _isBouncing, short _amountOdBounces, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOdBounces;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    public void SetUpPierce(int _pierceAmount)
    { 
        pierceAmount = _pierceAmount;
    }

    public void SetUpSpin(bool _isSpinning, 
                          float _maxTravelDistance, 
                          float _spinDuration,
                          float _hitCooldown)
    {
        isSpinnnig = _isSpinning;
        spinDuration = _spinDuration;
        maxTravelDistance = _maxTravelDistance;
        hitCooldown = _hitCooldown;
    }
    #endregion

    #region SPIN - BOUNCE LOGIC

    private void SpinLogic()
    {
        if (isSpinnnig)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinnnig = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = GetCollider2D(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() is not null)
                            SworSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        if (!wasStopped) spinTimer = spinDuration;
        wasStopped = true;
        rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        //spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SworSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;

                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    #endregion


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) return;

        if(collision.GetComponent<Enemy>() is not null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SworSkillDamage(enemy);
        }


        SetUpTargetForBounce(collision);

        StuckInto(collision);

    }

    private void SworSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine(EntityContstant.FREEZETOMEFOR, freezeTimeDuration);
    }

    private void SetUpTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() is not null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = GetCollider2D(transform.position, bounceDetectRadius);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() is not null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private Collider2D[] GetCollider2D(Vector2 point, float radius) => Physics2D.OverlapCircleAll(point, radius);

    //sword stuck to ground or enemy
    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() is not null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinnnig)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        circleCollider.enabled = false;

        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0) return;

        animator.SetBool(EntityContstant.ROTATION, false);
        transform.parent = collision.transform;
    }
}
