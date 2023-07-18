using System;
using UnityEngine;

public class Clone : Skill
{
    [Header("Clone Info")]
    [SerializeField] private float cloneDuraiton;
    [SerializeField] private GameObject clonePrefab;
    [Space]
    [SerializeField] private bool canAttack;

    public void CreateClone(Transform _clonePosition)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuraiton, canAttack);
    }
}
