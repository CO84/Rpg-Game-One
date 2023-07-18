using UnityEngine;

public class SkeletonAnimationTrigger : MonoBehaviour
{
    private EnemySkeleton enemySkeleton => GetComponentInParent<EnemySkeleton>();

    void AnimationTrigger() => enemySkeleton.AnimationFinishTrigger();

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemySkeleton.attackCheck.position, enemySkeleton.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
                hit.GetComponent<Player>().Damage();
        }
    }

    private void OpenCounterWindow() => enemySkeleton.OpenCounterAttackWindow();

    private void CloseCounterWindow() => enemySkeleton.CloseCounterAttackWindow();
}
