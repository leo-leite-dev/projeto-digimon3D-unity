using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private HotbarController hotbar;

    [SerializeField]
    private Camera targetCamera;

    private PlayerControls controls;

    private System.Action<InputAction.CallbackContext>[] skillCallbacks;

    public void Initialize(PlayerControls controls)
    {
        this.controls = controls;

        RegisterCombatInputs();
    }

    private void Awake()
    {
        if (hotbar == null)
            TryGetComponent(out hotbar);

        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    private void OnDisable()
    {
        UnregisterCombatInputs();
    }

    private void RegisterCombatInputs()
    {
        skillCallbacks = new System.Action<InputAction.CallbackContext>[5];

        skillCallbacks[0] = ctx => HandleSkillInput(ctx, 0);
        skillCallbacks[1] = ctx => HandleSkillInput(ctx, 1);
        skillCallbacks[2] = ctx => HandleSkillInput(ctx, 2);
        skillCallbacks[3] = ctx => HandleSkillInput(ctx, 3);
        skillCallbacks[4] = ctx => HandleSkillInput(ctx, 4);

        controls.Combat.Skill1.performed += skillCallbacks[0];
        controls.Combat.Skill2.performed += skillCallbacks[1];
        controls.Combat.Skill3.performed += skillCallbacks[2];
        controls.Combat.Skill4.performed += skillCallbacks[3];
        controls.Combat.Skill5.performed += skillCallbacks[4];
    }

    private void UnregisterCombatInputs()
    {
        if (skillCallbacks == null || controls == null)
            return;

        controls.Combat.Skill1.performed -= skillCallbacks[0];
        controls.Combat.Skill2.performed -= skillCallbacks[1];
        controls.Combat.Skill3.performed -= skillCallbacks[2];
        controls.Combat.Skill4.performed -= skillCallbacks[3];
        controls.Combat.Skill5.performed -= skillCallbacks[4];
    }

    private void HandleSkillInput(InputAction.CallbackContext ctx, int slotIndex)
    {
        if (!ctx.performed)
            return;

        if (hotbar == null || targetCamera == null)
        {
            Debug.LogWarning("[PlayerInputController] Referências inválidas");
            return;
        }

        var context = BuildContext();

        // hotbar.UseSlot(slotIndex, context);
    }

    private CombatContext BuildContext()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = targetCamera.ScreenPointToRay(mousePosition);

        return new CombatContext(targetCamera.transform.position, ray.direction);
    }
}
