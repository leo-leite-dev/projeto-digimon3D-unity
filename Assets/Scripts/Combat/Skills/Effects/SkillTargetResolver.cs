using UnityEngine;

public class SkillTargetResolver
{
    public Transform Resolve(Transform target)
    {
        if (target == null)
            return null;

        var enemy = target.GetComponent<DigimonEnemy>();

        if (enemy != null && enemy.TargetPoint != null)
            return enemy.TargetPoint;

        return target;
    }
}
