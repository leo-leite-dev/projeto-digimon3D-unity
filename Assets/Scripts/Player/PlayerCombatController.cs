using UnityEngine;

public class PlayerCombatController : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private TargetSystem targetSystem;

    private DigimonFollow currentDigimon;
    public DigimonFollow CurrentDigimon => currentDigimon;

    protected override void Validate()
    {
        if (targetSystem == null)
            targetSystem = GetComponent<TargetSystem>();
    }

    public void UseSkill(DigimonSkill skill)
    {
        if (skill == null)
            return;

        if (currentDigimon == null)
            return;

        if (targetSystem == null)
            return;

        GameObject target = targetSystem.GetCurrentTarget();

        if (target == null)
            return;

        currentDigimon.RequestSkillUse(skill, target);
    }

    public void SetDigimon(DigimonFollow digimon)
    {
        currentDigimon = digimon;
    }
}
