using System;
using UnityEngine;

// =========================
// ENEMY HEALTH (Domain)
// =========================
public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 10;

    private int currentHealth;
    private Digimon lastAttacker;

    public bool IsDead => currentHealth <= 0;

    public static event Action<EnemyDeathContext> OnEnemyKilled;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Digimon attacker)
    {
        if (IsDead)
            return;

        lastAttacker = attacker;
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        var context = BuildDeathContext();

        OnEnemyKilled?.Invoke(context);

        Destroy(gameObject);
    }

    EnemyDeathContext BuildDeathContext()
    {
        return new EnemyDeathContext
        {
            EnemyGO = gameObject,
            EnemyDigimon = GetComponent<Digimon>(),
            Enemy = GetComponent<DigimonEnemy>(),
            Killer = lastAttacker,
        };
    }
}

// =========================
// CONTEXT
// =========================
public class EnemyDeathContext
{
    public GameObject EnemyGO;
    public Digimon EnemyDigimon;
    public DigimonEnemy Enemy;
    public Digimon Killer;
}

// =========================
// EXPERIENCE SERVICE
// =========================
public class ExperienceService
{
    public void Handle(EnemyDeathContext context)
    {
        if (context.Killer == null || context.EnemyDigimon == null)
            return;

        int xp = ExperienceCalculator.CalculateExpGain(context.Killer, context.EnemyDigimon);

        context.Killer.AddExperience(xp);

        Debug.Log(
            $"========== ENEMY DEFEATED ==========\n"
                + $"{context.Killer.Name} derrotou {context.EnemyDigimon.Name}\n"
                + $"Level Attacker: {context.Killer.Level}\n"
                + $"Level Enemy: {context.EnemyDigimon.Level}\n"
                + $"XP ganho: {xp}"
        );
    }
}

// =========================
// SPAWNER LISTENER
// =========================
public class EnemySpawnListener : MonoBehaviour
{
    private EnemySpawner spawner;

    public void Initialize(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    void OnEnable()
    {
        EnemyHealth.OnEnemyKilled += Handle;
    }

    void OnDisable()
    {
        EnemyHealth.OnEnemyKilled -= Handle;
    }

    void Handle(EnemyDeathContext context)
    {
        if (spawner != null)
            spawner.NotifyEnemyDeath(context.EnemyGO);
    }
}

// =========================
// EXPERIENCE LISTENER
// =========================
public class EnemyExperienceListener : MonoBehaviour
{
    private ExperienceService service = new ExperienceService();

    void OnEnable()
    {
        EnemyHealth.OnEnemyKilled += Handle;
    }

    void OnDisable()
    {
        EnemyHealth.OnEnemyKilled -= Handle;
    }

    void Handle(EnemyDeathContext context)
    {
        service.Handle(context);
    }
}
