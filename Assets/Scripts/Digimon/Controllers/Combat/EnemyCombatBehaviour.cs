using UnityEngine;

public class EnemyCombatBehaviour : MonoBehaviour
{
    private DigimonBattleController controller;
    private DigimonAttack attack;
    private BattleContext context;

    private float decisionTimer;
    private float decisionInterval = 2f;

    private bool isInitialized;

    public void Initialize(DigimonBattleController controller, DigimonAttack attack)
    {
        this.controller = controller;
        this.attack = attack;

        TryFinalizeInitialization();
    }

    public void SetContext(BattleContext context)
    {
        this.context = context;

        TryFinalizeInitialization();
    }

    private void TryFinalizeInitialization()
    {
        if (controller == null || attack == null || context == null)
            return;

        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized)
            return;

        if (context.IsFinished)
            return;

        decisionTimer += Time.deltaTime;

        if (decisionTimer >= decisionInterval)
        {
            decisionTimer = 0f;
            MakeDecision();
        }
    }

    private void MakeDecision()
    {
        if (attack.IsCasting)
            return;

        var digimon = attack.Digimon;

        if (digimon == null || digimon.Data == null)
            return;

        var skills = digimon.Data.skills;

        if (skills == null || skills.Count == 0)
            return;

        int index = Random.Range(0, skills.Count);

        controller.UseSkill(index);
    }
}
