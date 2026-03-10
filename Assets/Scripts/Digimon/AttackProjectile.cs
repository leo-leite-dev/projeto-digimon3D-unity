using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    private Digimon attacker;

    public float speed = 10f;
    public float hitDistance = 0.2f;

    private Transform target;
    private int damage;

    public void Setup(Transform newTarget, int newDamage, Digimon newAttacker)
    {
        target = newTarget;
        damage = newDamage;
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
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
    }

    bool HasHitTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        return distance <= hitDistance;
    }

    void Hit()
    {
        EnemyHealth enemy = target.GetComponentInParent<EnemyHealth>();

        if (enemy != null && !enemy.IsDead)
            enemy.TakeDamage(damage, attacker);

        Destroy(gameObject);
    }
}
