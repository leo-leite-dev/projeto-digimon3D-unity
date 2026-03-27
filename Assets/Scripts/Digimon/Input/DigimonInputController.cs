using UnityEngine;
using UnityEngine.InputSystem;

public class DigimonInputController : MonoBehaviour
{
    private DigimonBattleController battleController;
    private BattleContext context;
    private PlayerControls controls;

    private System.Action<InputAction.CallbackContext>[] skillCallbacks;

    private bool isInitialized;

    public void Initialize(PlayerControls controls, DigimonBattleController battleController)
    {
        this.controls = controls;
        this.battleController = battleController;

        TryFinalizeInitialization();
    }

    public void SetContext(BattleContext context)
    {
        this.context = context;
        TryFinalizeInitialization();
    }

    private void TryFinalizeInitialization()
    {
        if (controls == null || battleController == null || context == null)
            return;

        isInitialized = true;
    }

    private void OnEnable()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("[DigimonInputController] Não inicializado ainda");
            return;
        }

        RegisterInputs();
    }

    private void OnDisable()
    {
        UnregisterInputs();
    }

    private void RegisterInputs()
    {
        skillCallbacks = new System.Action<InputAction.CallbackContext>[5];

        skillCallbacks[0] = ctx => HandleSkill(ctx, 0);
        skillCallbacks[1] = ctx => HandleSkill(ctx, 1);
        skillCallbacks[2] = ctx => HandleSkill(ctx, 2);
        skillCallbacks[3] = ctx => HandleSkill(ctx, 3);
        skillCallbacks[4] = ctx => HandleSkill(ctx, 4);

        controls.Combat.Skill1.performed += skillCallbacks[0];
        controls.Combat.Skill2.performed += skillCallbacks[1];
        controls.Combat.Skill3.performed += skillCallbacks[2];
        controls.Combat.Skill4.performed += skillCallbacks[3];
        controls.Combat.Skill5.performed += skillCallbacks[4];
    }

    private void UnregisterInputs()
    {
        if (skillCallbacks == null || controls == null)
            return;

        controls.Combat.Skill1.performed -= skillCallbacks[0];
        controls.Combat.Skill2.performed -= skillCallbacks[1];
        controls.Combat.Skill3.performed -= skillCallbacks[2];
        controls.Combat.Skill4.performed -= skillCallbacks[3];
        controls.Combat.Skill5.performed -= skillCallbacks[4];
    }

    private void HandleSkill(InputAction.CallbackContext ctx, int slotIndex)
    {
        if (!ctx.performed)
            return;

        if (!isInitialized)
            return;

        if (context.IsFinished)
            return;

        battleController.UseSkill(slotIndex);
    }
}
