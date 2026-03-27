using UnityEngine;

public class SkillSelector
{
    public DigimonSkill Select(Digimon digimon)
    {
        var skills = digimon.Skills;

        if (skills == null || skills.Count == 0)
            return null;

        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, skills.Count);
            var skill = skills[index];

            if (skill != null)
                return skill;
        }

        return null;
    }
}
