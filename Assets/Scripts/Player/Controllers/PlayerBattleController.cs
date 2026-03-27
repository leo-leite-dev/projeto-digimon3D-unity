using UnityEngine;

public class PlayerBattleController : MonoBehaviour
{
    private BattleContext context;

    private bool isInitialized;

    public void Initialize()
    {
        isInitialized = true;

        Debug.Log("[PlayerBattleController] Inicializado");
    }

    public void SetContext(BattleContext context)
    {
        this.context = context;
    }

    // public void UseSkill(PlayerSkill skill)
    // {
    //     if (!isInitialized)
    //     {
    //         Debug.LogWarning("[PlayerBattleController] Não inicializado");
    //         return;
    //     }

    //     if (context == null)
    //     {
    //         Debug.LogError("[PlayerBattleController] BattleContext não definido");
    //         return;
    //     }

    //     if (context.IsFinished)
    //         return;

    //     if (skill == null)
    //     {
    //         Debug.LogWarning("[PlayerBattleController] Skill nula");
    //         return;
    //     }

    //     ExecuteSkill(skill);
    // }

    // private void ExecuteSkill(PlayerSkill skill)
    // {
    //     var playerDigimon = context.Player;

    //     if (playerDigimon == null)
    //     {
    //         Debug.LogError("[PlayerBattleController] Digimon do player não encontrado");
    //         return;
    //     }

    //     skill.Execute(playerDigimon, context);

    //     Debug.Log($"✨ Player usou skill: {skill.name}");
    // }
}
