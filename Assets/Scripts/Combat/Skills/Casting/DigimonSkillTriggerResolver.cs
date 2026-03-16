public class DigimonSkillTriggerResolver
{
    private readonly DigimonSkillTriggerFormatter formatter;

    public DigimonSkillTriggerResolver(DigimonSkillTriggerFormatter formatter)
    {
        this.formatter = formatter;
    }

    public string Resolve(DigimonSkill skill)
    {
        if (skill == null)
            return null;

        if (!string.IsNullOrWhiteSpace(skill.animationTrigger))
            return formatter.Format(skill.animationTrigger);

        if (!string.IsNullOrWhiteSpace(skill.skillName))
            return formatter.Format(skill.skillName);

        return null;
    }
}
