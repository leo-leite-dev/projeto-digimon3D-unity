using System.Collections.Generic;
using UnityEngine;

public class SkillCooldownTracker
{
    private readonly Dictionary<DigimonSkill, float> lastUseTimes = new();

    public bool IsOnCooldown(DigimonSkill skill)
    {
        if (skill == null)
            return false;

        if (!lastUseTimes.TryGetValue(skill, out float lastUseTime))
            return false;

        return Time.time < lastUseTime + skill.cooldown;
    }

    public float GetRemainingCooldown(DigimonSkill skill)
    {
        if (skill == null)
            return 0f;

        if (!lastUseTimes.TryGetValue(skill, out float lastUseTime))
            return 0f;

        float endTime = lastUseTime + skill.cooldown;
        return Mathf.Max(0f, endTime - Time.time);
    }

    public void RegisterUse(DigimonSkill skill)
    {
        if (skill == null)
            return;

        lastUseTimes[skill] = Time.time;
    }
}
