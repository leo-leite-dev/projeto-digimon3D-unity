using System.Collections.Generic;
using UnityEngine;

public class DigimonAttack : MonoBehaviour
{
    private Digimon digimon;
    public TargetSystem targetSystem;
    public List<DigimonSkill> skills;

    public GameObject projectilePrefab;
    public Transform firePoint;

    private float cooldownTimer;

    void Awake()
    {
        digimon = GetComponent<Digimon>();
    }

    void Update()
    {
        UpdateCooldown();
        HandleAttack();
    }

    void UpdateCooldown()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    void HandleAttack()
    {
        if (targetSystem == null)
            return;

        if (skills == null || skills.Count == 0)
            return;

        GameObject target = GetValidTarget();
        if (target == null)
            return;

        DigimonSkill skill = skills[0];

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > skill.range)
            return;

        if (cooldownTimer > 0f)
            return;

        LookTarget(target);

        ExecuteAttack(skill, target);
    }

    GameObject GetValidTarget()
    {
        GameObject target = targetSystem.currentTarget;

        if (target == null)
            return null;

        EnemyHealth enemy = target.GetComponentInParent<EnemyHealth>();

        if (enemy == null || enemy.IsDead)
        {
            targetSystem.currentTarget = null;
            return null;
        }

        return enemy.gameObject;
    }

    void LookTarget(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;

        direction.y = 0f;

        if (direction == Vector3.zero)
            return;

        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    void ExecuteAttack(DigimonSkill skill, GameObject target)
    {
        if (skill.attackType == AttackType.Magic)
            ShootProjectile(skill, target);
        else
            DealMeleeDamage(skill, target);

        cooldownTimer = skill.cooldown;
    }

    void DealMeleeDamage(DigimonSkill skill, GameObject target)
    {
        EnemyHealth enemy = target.GetComponentInParent<EnemyHealth>();

        if (enemy == null)
            return;

        Digimon defender = target.GetComponent<Digimon>();

        if (defender == null)
        {
            enemy.TakeDamage(skill.damage, digimon);
            return;
        }

        int damage = CombatCalculator.CalculateDamage(digimon, defender, skill.attackType);

        if (damage <= 0)
        {
            HandleMiss();
            return;
        }

        enemy.TakeDamage(damage, digimon);
    }

    void ShootProjectile(DigimonSkill skill, GameObject target)
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        AttackProjectile proj = projectile.GetComponent<AttackProjectile>();

        if (proj != null)
        {
            Digimon defender = target.GetComponent<Digimon>();

            int damage = skill.damage;

            if (defender != null)
                damage = CombatCalculator.CalculateDamage(digimon, defender, skill.attackType);

            if (damage <= 0)
            {
                HandleMiss();
                return;
            }

            proj.Setup(target.transform, damage, digimon);
        }
    }

    void HandleMiss()
    {
        Debug.Log($"{digimon.Name} MISS!");

        // futuramente pode adicionar:
        // FloatingText "MISS"
        // efeito visual
        // som
    }
}
