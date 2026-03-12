using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    private DigimonAttack attack;

    public TargetSystem targetSystem;

    private KeyCode[] skillKeys =
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
    };

    void Awake()
    {
        if (targetSystem == null)
            targetSystem = FindFirstObjectByType<TargetSystem>();
    }

    void Update()
    {
        if (attack == null)
            return;

        if (targetSystem == null || targetSystem.currentTarget == null)
            return;

        for (int i = 0; i < skillKeys.Length; i++)
        {
            if (Input.GetKeyDown(skillKeys[i]))
                attack.UseSkill(i, targetSystem.currentTarget);
        }
    }

    public void SetDigimon(DigimonAttack newAttack)
    {
        attack = newAttack;

        if (attack == null)
        {
            Debug.LogWarning("PlayerCombatController: DigimonAttack não configurado.");
            return;
        }

        Debug.Log($"Digimon equipado para combate: {attack.name}");
    }
}
