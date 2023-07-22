using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        if(cooldownTimer > -1)
         cooldownTimer -= Time.deltaTime;
        
    }

    public virtual bool CanUseSkill()
    {
        if(cooldownTimer < 0)
        {
            //use skill
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    public virtual void UseSkill()
    {
        //do some skill specific things
    }
}
