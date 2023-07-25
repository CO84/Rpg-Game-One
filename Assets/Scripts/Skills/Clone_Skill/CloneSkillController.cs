using Assets.Scripts.Contants;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private float colorLoosingSpeed;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform closestEnemy;

    private float cloneTimer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer < 0)
        {
           spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - (Time.deltaTime * colorLoosingSpeed));
            
            if(spriteRenderer.color.a <= 0)
                Destroy(gameObject);
        }
    }

    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack)
    {
        if (_canAttack)
            animator.SetInteger(EntityContstant.ATTACK_NUMBER, Random.Range(1,3));

        transform.position = _newTransform.position;
        cloneTimer = _cloneDuration;


        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = GetCollider2D(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }

    private void FaceClosestTarget()
    {
        Collider2D[] colliders = GetCollider2D(transform.position, 25);

        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() is not null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
                   
            }
          
        }

        if(closestEnemy is not null)
        {
            if (transform.position.x > closestEnemy.position.x)
                transform.Rotate(0, 180, 0);
        }
    }

    private Collider2D[] GetCollider2D(Vector2 point, float radius) => Physics2D.OverlapCircleAll(point, radius);
}
