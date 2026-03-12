using UnityEngine;

public class DigimonAttack : MonoBehaviour
{
    [Header("References")]
    public TargetSystem targetSystem;
    public Transform firePoint;
    public Digimon digimon;

    public bool IsAttacking { get; private set; }
    private bool combatActive;
    private int currentSkillIndex = 0;

    private float lastAttackTime;

    void Awake()
    {
        if (targetSystem == null)
            targetSystem = FindFirstObjectByType<TargetSystem>();

        if (digimon == null)
            digimon = GetComponentInParent<Digimon>();
    }

    void Update()
    {
        if (targetSystem == null || digimon == null)
            return;

        HandleCombat();
    }

    void HandleCombat()
    {
        GameObject target = targetSystem.currentTarget;

        if (target == null)
        {
            combatActive = false;
            IsAttacking = false;
            return;
        }

        IsAttacking = combatActive;

        if (!combatActive)
            return;

        TryAttack(target);
    }

    public void UseSkill(int index, GameObject target)
    {
        if (digimon.data.skills == null)
            return;

        if (index < 0 || index >= digimon.data.skills.Count)
            return;

        DigimonSkill skill = digimon.data.skills[index];

        if (skill == null)
            return;

        currentSkillIndex = index;
        combatActive = true;

        TryAttack(target);
    }

    void MoveToTarget(GameObject target)
    {
        float speed = 5f;

        DigimonSkill skill = digimon.data.skills[currentSkillIndex];

        Vector3 direction = (target.transform.position - transform.position).normalized;

        float stopDistance = skill.range * 0.9f;

        Vector3 desiredPosition = target.transform.position - direction * stopDistance;

        transform.position = Vector3.MoveTowards(
            transform.position,
            desiredPosition,
            speed * Time.deltaTime
        );
    }

    void Attack(GameObject target, DigimonSkill skill)
    {
        RotateToTarget(target);

        Debug.Log($"{digimon.Name} usou {skill.skillName}");

        if (skill.projectilePrefab == null)
            return;

        GameObject projectile = Instantiate(
            skill.projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        AttackProjectile proj = projectile.GetComponent<AttackProjectile>();

        if (proj != null)
            proj.Setup(target.transform, skill, digimon);
    }

    void TryAttack(GameObject target)
    {
        RotateToTarget(target);

        if (digimon.data.skills == null || digimon.data.skills.Count == 0)
            return;

        if (currentSkillIndex >= digimon.data.skills.Count)
            return;

        DigimonSkill skill = digimon.data.skills[currentSkillIndex];

        if (Time.time < lastAttackTime + skill.cooldown)
        {
            Debug.Log("Skill em cooldown");
            return;
        }

        float sqrDistance = (target.transform.position - transform.position).sqrMagnitude;

        float attackRange = skill.range;

        if (sqrDistance > attackRange * attackRange)
        {
            MoveToTarget(target);
            return;
        }
        Attack(target, skill);

        lastAttackTime = Time.time;
    }

    void RotateToTarget(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            10f * Time.deltaTime
        );
    }
}
