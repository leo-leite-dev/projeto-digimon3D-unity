using UnityEngine;

public class SkillEffectSpawnCoordinator
{
    private readonly SkillEffectSpawner spawner;

    public SkillEffectSpawnCoordinator(SkillEffectSpawner spawner)
    {
        this.spawner = spawner;
    }

    public SkillEffect Spawn(
        DigimonSkill skill,
        Transform target,
        Vector3 position,
        Quaternion rotation
    )
    {
        if (skill == null || skill.projectilePrefab == null || target == null)
            return null;

        var go = spawner.Spawn(skill.projectilePrefab, position, rotation);

        var effect = go.GetComponent<SkillEffect>();

        if (effect == null)
            return null;

        effect.Setup(target, skill);

        return effect;
    }
}
