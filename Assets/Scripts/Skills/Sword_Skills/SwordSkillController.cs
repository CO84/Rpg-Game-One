using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12;

    private Animator animator;
    private new Rigidbody2D rigidbody;
    private CircleCollider2D circleCollider;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private short swordHideDistance = 1;


    // TODO Awake çalýþmýyordu start ekleyince çalýþmaya baþladý daha sonra ihtiyaç kalmadý
    // transform.right = rigidbody.velocity; doðru çalýþmaya baþladý
    // unity hata veriyor bug?
    //private void Start()
    //{
    //    //animator = GetComponentInChildren<Animator>();
    //    //rigidbody = GetComponent<Rigidbody2D>();
    //    //circleCollider = GetComponent<CircleCollider2D>();
    //}

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(Vector2 _direction, float _gravityScale, Player _player)
    {
        player = _player;
        rigidbody.velocity = _direction;
        rigidbody.gravityScale = _gravityScale;


    }

    public void ReturnSword()
    {
        rigidbody.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rigidbody.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < swordHideDistance )
                player.ClearTheSword();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canRotate = false;
        circleCollider.enabled = false;

        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;

    }
}
