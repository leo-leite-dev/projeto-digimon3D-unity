using UnityEngine;

[CreateAssetMenu(menuName = "Digimon/Skill")]
public class DigimonSkill : ScriptableObject
{
    [Header("Info")]
    public string skillName;

    [Header("Core")]
    public AttackType attackType;

    public float range = 3f;
    public int damage = 10;
    public float cooldown = 1f;

    [Header("Type")]
    public SkillType skillType;

    [Header("Projectile")]
    public GameObject effectPrefab;
    public ProjectileMovementType projectileMovement = ProjectileMovementType.MoveToTarget;
    public float projectileSpeed = 10f;

    [Header("Timing")]
    public float damageDelay = 0f;
    public float lifeTime = 2f;

    [Header("Animation")]
    public string animationTrigger;

    [Header("Advanced Projectile Behaviour")]
    public bool useDelayedMovement;
    public ProjectileMovementType initialMovement = ProjectileMovementType.Static;
    public ProjectileMovementType delayedMovement = ProjectileMovementType.MoveToTarget;

    public bool IsEffect => skillType == SkillType.Effect;
    public bool IsDirect => skillType == SkillType.Direct;
}
