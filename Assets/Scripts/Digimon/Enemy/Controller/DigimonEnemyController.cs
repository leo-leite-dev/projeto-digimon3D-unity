using UnityEngine;

public class DigimonEnemyController : MonoBehaviour
{
    private IAttackCapability attack;
    private IMovementCapability movement;

    [SerializeField]
    private DigimonCapabilityProvider provider;

    [SerializeField]
    private DigimonSkill skill;
    private Transform target;

    private void Awake()
    {
        attack = provider.Get<IAttackCapability>();
        movement = provider.Get<IMovementCapability>();
    }

    private void Update()
    {
        if (target == null)
            return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < 5f)
        {
            movement?.Stop();

            if (attack?.CanUseSkill(skill, target.gameObject) == true)
                attack.UseSkill(skill, target.gameObject);
        }
        else
        {
            movement?.MoveTo(target);
        }
    }
}
