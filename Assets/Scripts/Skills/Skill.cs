using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

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

        //TODO Debul kalkacak
        Debug.Log("Skill is cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        //do some skill specific things
    }
}
