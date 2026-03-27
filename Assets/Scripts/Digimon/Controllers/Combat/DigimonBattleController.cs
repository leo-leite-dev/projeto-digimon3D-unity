using UnityEngine;

public class DigimonBattleController : MonoBehaviour
{
    private DigimonAttack attack;
    private BattleContext context;

    private DigimonMovement movement;

    private bool isInitialized;

    public void Initialize(DigimonAttack attack)
    {
        this.attack = attack;
        TryFinalizeInitialization();
    }

    public void SetContext(BattleContext context)
    {
        this.context = context;

        if (context != null)
            context.OnBattleFinished += HandleBattleFinished;

        TryFinalizeInitialization();
    }

    private void Awake()
    {
        TryGetComponent(out movement);
    }

    private void TryFinalizeInitialization()
    {
        if (attack == null || context == null)
            return;

        isInitialized = true;
    }

    private void HandleBattleFinished(BattleState state)
    {
        if (movement == null)
            return;

        movement.StopMovement();
        movement.ClearAllMovementLocks();
    }

    public void UseSkill(int slotIndex)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("⚠️ DigimonBattleController não inicializado");
            return;
        }

        if (context == null || context.IsFinished)
            return;

        var digimon = attack.Digimon;

        if (digimon == null)
        {
            Debug.LogError("❌ Digimon não encontrado");
            return;
        }

        var data = digimon.Data;

        if (data == null)
        {
            Debug.LogError("❌ DigimonData não definido");
            return;
        }

        var skills = data.skills;

        if (skills == null || skills.Count == 0)
        {
            Debug.LogWarning("⚠️ Digimon não possui skills");
            return;
        }

        if (slotIndex < 0 || slotIndex >= skills.Count)
        {
            Debug.LogWarning($"⚠️ Slot inválido: {slotIndex}");
            return;
        }

        var skill = skills[slotIndex];

        if (skill == null)
        {
            Debug.LogWarning($"⚠️ Skill no slot {slotIndex} é nula");
            return;
        }

        attack.TryUseSkill(skill, context);
    }
}
