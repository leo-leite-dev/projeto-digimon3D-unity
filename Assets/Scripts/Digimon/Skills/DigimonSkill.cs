using UnityEngine;

[CreateAssetMenu(menuName = "Digimon/Skill")]
public class DigimonSkill : ScriptableObject
{
    public string skillName;

    [Header("Combat")]
    public AttackType attackType;
    public AttackRange rangeType;

    [Header("Animation")]
    public string animationTrigger;

    [Header("Skill Stats")]
    public float range = 3f;
    public int damage = 3;
    public float cooldown = 1f;

    [Header("Hit")]
    public SkillHitTriggerType hitTriggerType = SkillHitTriggerType.Direct;

    [Header("Effect / Projectile")]
    public GameObject projectilePrefab;
    public ProjectileMovementType projectileMovement = ProjectileMovementType.MoveToTarget;
    public float projectileSpeed = 10f;

    [Header("Effect Timing")]
    public float hitDelay = 0f;
    public float lifeTime = 2f;

    [Header("Finish")]
    public SkillFinishMode finishMode = SkillFinishMode.Animation;
}
