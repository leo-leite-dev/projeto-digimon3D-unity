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

    [SerializeField]
    private float maxLifetime = 5f;

    private Transform target;
    private DigimonSkill skill;

    private ProjectileMovementType currentMovement;

    private float spawnTime;
    private bool wasEndedByAnimation;
    private bool hasReachedTarget;
    private bool hasNotifiedImpact;
    private bool canImpact;
    private bool isEnding;

    private Vector3 lastDirection;

    public event Action<GameObject> OnImpact;
    public event Action OnEffectEnded;

    public void Setup(Transform newTarget, DigimonSkill newSkill)
    {
        OnImpact = null;
        OnEffectEnded = null;

        var provider = newTarget.GetComponentInChildren<HitPointProvider>();
        target = provider != null ? provider.GetHitPoint() : newTarget;

        skill = newSkill;

        hasReachedTarget = false;
        hasNotifiedImpact = false;
        wasEndedByAnimation = false;
        isEnding = false;
        canImpact = false;

        spawnTime = Time.time;
        currentSpeed = skill != null ? skill.projectileSpeed : 10f;

        currentMovement = ProjectileMovementType.Static;
    }

    private void Update()
    {
        // 🔥 ESSENCIAL
        if (isEnding)
            return;

        if (skill == null)
        {
            Debug.Log("⚠️ Skill null → encerrando efeito");
            TryEndEffect();
            return;
        }

        if (target == null)
        {
            Debug.Log("⚠️ Target perdido → encerrando efeito");
            TryEndEffect();
            return;
        }

        UpdateMovement();
        HandleLifetime();
        HandleMaxLifetime();
    }

    public void SetMovementType(ProjectileMovementType movementType)
    {
        currentMovement = movementType;

        if (movementType == ProjectileMovementType.MoveToTarget)
        {
            canImpact = false;
            StartCoroutine(EnableImpactNextFrame());
        }
    }

    private void UpdateMovement()
    {
        if (currentMovement == ProjectileMovementType.MoveToTarget)
            UpdateMoveToTarget();
    }

    private void UpdateMoveToTarget()
    {
        if (hasReachedTarget || isEnding)
            return;

        Vector3 toTarget = target.position - transform.position;
        Vector3 direction = toTarget.normalized;

        if (toTarget.sqrMagnitude > 0.01f)
            lastDirection = direction;

        transform.position += direction * currentSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        if (!canImpact)
            return;

        float sqrDistance = toTarget.sqrMagnitude;
        float hitDistanceSqr = hitDistance * hitDistance;

        if (sqrDistance > hitDistanceSqr)
            return;

        hasReachedTarget = true;

        NotifyImpact();
        TryEndEffect();
    }

    private IEnumerator EnableImpactNextFrame()
    {
        yield return null;

        // 🔥 evita bug tardio
        if (isEnding)
            yield break;

        canImpact = true;
    }

    private void NotifyImpact()
    {
        if (hasNotifiedImpact || target == null || isEnding)
            return;

        hasNotifiedImpact = true;

        Debug.Log("💥 Impact detectado");

        OnImpact?.Invoke(target.gameObject);
    }

    private void HandleLifetime()
    {
        if (wasEndedByAnimation || isEnding)
            return;

        if (currentMovement == ProjectileMovementType.MoveToTarget)
            return;

        if (useLifetimeForStaticEffects && Time.time >= spawnTime + skill.lifeTime)
            TryEndEffect();
    }

    private void HandleMaxLifetime()
    {
        if (isEnding)
            return;

        if (Time.time >= spawnTime + maxLifetime)
        {
            Debug.Log("⏱️ MaxLifetime atingido → encerrando efeito");
            TryEndEffect();
        }
    }

    public void EndEffectFromAnimation()
    {
        wasEndedByAnimation = true;
        TryEndEffect();
    }

    private void TryEndEffect()
    {
        if (isEnding)
            return;

        isEnding = true;

        Debug.Log("🏁 Effect finalizando");

        OnEffectEnded?.Invoke();

        StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
        float duration = 0.8f;
        float time = 0f;

        var renderers = GetComponentsInChildren<Renderer>();
        Vector3 initialScale = transform.localScale;

        if (lastDirection == Vector3.zero)
            lastDirection = transform.forward;

        yield return new WaitForSeconds(0.05f);

        while (time < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, time / duration);

            transform.position += lastDirection * currentSpeed * 0.3f * (1f - t) * Time.deltaTime;

            foreach (var r in renderers)
            {
                foreach (var mat in r.materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        var color = mat.color;
                        color.a = Mathf.Lerp(1f, 0f, t);
                        mat.color = color;
                    }
                }
            }

            transform.localScale = Vector3.Lerp(initialScale, initialScale * 0.3f, t);

            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
