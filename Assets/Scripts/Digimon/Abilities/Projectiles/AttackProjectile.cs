using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    private Digimon attacker;

    public float speed = 10f;
    public float hitDistance = 0.2f;

    private Transform target;
    private DigimonSkill skill;

    public void Setup(Transform newTarget, DigimonSkill newSkill, Digimon newAttacker)
    {
        target = newTarget;
        skill = newSkill;
        attacker = newAttacker;
    }

    void Update()
    {
        if (!HasTarget())
            return;

        MoveToTarget();

        if (HasHitTarget())
            Hit();
    }

    bool HasTarget()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return false;
        }

        return true;
    }

    void MoveToTarget()
    {
        transform.LookAt(target);

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
    }

    bool HasHitTarget()
    {
        float sqrDistance = (transform.position - target.position).sqrMagnitude;

        return sqrDistance <= hitDistance * hitDistance;
    }

    void Hit()
    {
        if (skill == null)
        {
            Debug.LogError("Skill não foi passada para o projectile!");
            Destroy(gameObject);
            return;
        }

        IDamageable damageable = target.GetComponentInParent<IDamageable>();
        Digimon defender = target.GetComponentInParent<Digimon>();

        if (damageable != null && defender != null)
        {
            Debug.Log($"Attacker stats null? {attacker.stats == null}");
            Debug.Log($"Defender stats null? {defender.stats == null}");
            Debug.Log($"Skill null? {skill == null}");

            int damage = CombatCalculator.CalculateDamage(attacker, defender, skill);

            damageable.TakeDamage(damage, attacker);
        }

        Destroy(gameObject);
    }
}
