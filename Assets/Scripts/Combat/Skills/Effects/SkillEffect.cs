using System;
using System.Collections;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    private float currentSpeed;

    [SerializeField]
    private float hitDistance = 1.5f;

    [Header("Behavior")]
    [SerializeField]
    private bool useLifetimeForStaticEffects = false;

    private Transform target;
    private DigimonSkill skill;

    private ProjectileMovementType currentMovement;

    private float spawnTime;
    private bool wasEndedByAnimation;
    private bool hasReachedTarget;
    private bool hasNotifiedImpact;

    public Action OnImpact;
    private bool canImpact;

    public void Setup(Transform newTarget, DigimonSkill newSkill)
    {
        target = newTarget;
        skill = newSkill;

        currentMovement = skill != null ? skill.projectileMovement : ProjectileMovementType.Static;

        hasReachedTarget = false;
        hasNotifiedImpact = false;
        wasEndedByAnimation = false;

        spawnTime = Time.time;
        currentSpeed = skill != null ? skill.projectileSpeed : 10f;
    }

    private void Update()
    {
        if (skill == null || target == null)
            return;

        switch (currentMovement)
        {
            case ProjectileMovementType.MoveToTarget:
                UpdateMoveToTarget();
                break;

            case ProjectileMovementType.Static:
                UpdateStatic();
                break;

            case ProjectileMovementType.Beam:
                UpdateBeam();
                break;
        }

        HandleLifetime();
    }

    private void UpdateMoveToTarget()
    {
        transform.LookAt(target);

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            currentSpeed * Time.deltaTime
        );

        if (!canImpact)
            return;

        float sqrDistance = (transform.position - target.position).sqrMagnitude;
        float hitDistanceSqr = hitDistance * hitDistance;

        if (sqrDistance > hitDistanceSqr || hasReachedTarget)
            return;

        hasReachedTarget = true;

        NotifyImpact();
        EndEffect();
    }

    private IEnumerator EnableImpactNextFrame()
    {
        yield return null;
        canImpact = true;
    }

    private void UpdateStatic() { }

    private void UpdateBeam() { }

    public void SwitchToMoveToTarget()
    {
        currentMovement = ProjectileMovementType.MoveToTarget;
        canImpact = false;

        StartCoroutine(EnableImpactNextFrame());
    }

    private void NotifyImpact()
    {
        if (hasNotifiedImpact)
            return;

        hasNotifiedImpact = true;

        Debug.Log("🔥 SkillEffect: IMPACT aconteceu");

        OnImpact?.Invoke();
    }

    private void HandleLifetime()
    {
        if (skill == null || wasEndedByAnimation)
            return;

        if (currentMovement == ProjectileMovementType.MoveToTarget)
            return;

        if (useLifetimeForStaticEffects && Time.time >= spawnTime + skill.lifeTime)
            EndEffect();
    }

    public void EndEffectFromAnimation()
    {
        wasEndedByAnimation = true;
        EndEffect();
    }

    private void EndEffect()
    {
        Destroy(gameObject);
    }
}
