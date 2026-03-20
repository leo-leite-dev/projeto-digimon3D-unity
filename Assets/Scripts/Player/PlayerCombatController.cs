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

    // public void UseSkill(DigimonSkill skill)
    // {
    //     if (skill == null)
    //         return;

    //     if (currentDigimon == null)
    //         return;

    //     if (targetSystem == null)
    //         return;

    //     GameObject target = targetSystem.GetCurrentTarget();

    //     if (target == null)
    //         return;

    //     currentDigimon.RequestSkill(skill, target);
    // }

    public void UseSkill(DigimonSkill skill)
    {
        Debug.Log("🟡 [FLOW] Combat.UseSkill chamado");

        if (skill == null)
        {
            Debug.Log("❌ Skill null");
            return;
        }

        if (currentDigimon == null)
        {
            Debug.Log("❌ currentDigimon null");
            return;
        }

        if (targetSystem == null)
        {
            Debug.Log("❌ targetSystem null");
            return;
        }

        GameObject target = targetSystem.GetCurrentTarget();

        if (target == null)
        {
            Debug.Log("❌ TARGET NULL (SEM ALVO)");
            return;
        }

        Debug.Log($"🟢 Target válido: {target.name}");

        currentDigimon.RequestSkill(skill, target);
    }

    public void SetDigimon(DigimonFollow digimon)
    {
        currentDigimon = digimon;
    }
}
