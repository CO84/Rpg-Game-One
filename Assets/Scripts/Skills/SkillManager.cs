using UnityEngine;

public sealed class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Dash DashSkill { get; private set; }
    public Clone CloneSkill { get; private set; }

    private void Awake()
    {
        if (instance is not null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        DashSkill = GetComponent<Dash>();
        CloneSkill = GetComponent<Clone>();
    }
}
