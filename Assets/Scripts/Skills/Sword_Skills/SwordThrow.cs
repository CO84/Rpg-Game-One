using Assets.Scripts.Skills.SkillEnums;
using System;
using UnityEngine;

public sealed class SwordThrow : Skill
{
    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration = .7f;
    [SerializeField] private float returnSpeed = 20;

    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefabs;
    [SerializeField] private Transform dotsParent;
    private GameObject[] dots;

    [Header("Bounce Info")]
    [SerializeField] private short bounceAmount = 4;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] private float pierceGravity = .1f;
    [SerializeField] private int pierceAmount;

    [Header("Spin Info")]
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = .2f;

    private Vector2 finalDirection;
    public SwordType swordType = SwordType.Regular;

    protected override void Start()
    {
        base.Start();
        GenerateDots();

        SetUpGravity();
    }

    private void SetUpGravity()
    {
        if(swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if(swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if(swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDirection = new Vector2(AimDirection().normalized.x * launchForce.x,
                                         AimDirection().normalized.y * launchForce.y);

        if(Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++) 
            {
                dots[i].transform.position = DotPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController swordController = newSword.GetComponent<SwordSkillController>();
        newSword.name = "Sword";

        if (swordType == SwordType.Bounce)
            swordController.SetUpBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordType.Pierce)
            swordController.SetUpPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            swordController.SetUpSpin(true, maxTravelDistance, spinDuration, hitCooldown);


        swordController.SetUpSword(finalDirection, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    #region SWORD AIM - DOTS

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
      

        return direction;
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefabs, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + (t *t) * .5f * (Physics2D.gravity * swordGravity);
        return position;
    }

    #endregion
}
