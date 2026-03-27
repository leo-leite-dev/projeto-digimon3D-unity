using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBattleInputController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private HotbarController hotbar;

    private PlayerControls controls;
    private System.Action<InputAction.CallbackContext>[] skillCallbacks;

    public void Initialize(PlayerControls controls)
    {
        this.controls = controls;
        RegisterInputs();
    }

    private void OnDisable()
    {
        UnregisterInputs();
    }

    private void RegisterInputs()
    {
        skillCallbacks = new System.Action<InputAction.CallbackContext>[5];

        skillCallbacks[0] = ctx => HandleSkill(0);
        skillCallbacks[1] = ctx => HandleSkill(1);
        skillCallbacks[2] = ctx => HandleSkill(2);
        skillCallbacks[3] = ctx => HandleSkill(3);
        skillCallbacks[4] = ctx => HandleSkill(4);

        controls.Combat.Skill1.performed += skillCallbacks[0];
        controls.Combat.Skill2.performed += skillCallbacks[1];
        controls.Combat.Skill3.performed += skillCallbacks[2];
        controls.Combat.Skill4.performed += skillCallbacks[3];
        controls.Combat.Skill5.performed += skillCallbacks[4];
    }

    private void UnregisterInputs()
    {
        if (controls == null || skillCallbacks == null)
            return;

        controls.Combat.Skill1.performed -= skillCallbacks[0];
        controls.Combat.Skill2.performed -= skillCallbacks[1];
        controls.Combat.Skill3.performed -= skillCallbacks[2];
        controls.Combat.Skill4.performed -= skillCallbacks[3];
        controls.Combat.Skill5.performed -= skillCallbacks[4];
    }

    private void HandleSkill(int index)
    {
        if (hotbar == null)
        {
            Debug.LogWarning("[BattleInput] Hotbar missing");
            return;
        }

        hotbar.UseSlot(index);
    }
}
