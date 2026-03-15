using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private HotbarController hotbar;

    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();

        if (hotbar == null)
            TryGetComponent(out hotbar);
    }

    void OnEnable()
    {
        controls.Enable();

        controls.Combat.Skill1.performed += OnSkill1;
        controls.Combat.Skill2.performed += OnSkill2;
        controls.Combat.Skill3.performed += OnSkill3;
        controls.Combat.Skill4.performed += OnSkill4;
        controls.Combat.Skill5.performed += OnSkill5;
    }

    void OnDisable()
    {
        controls.Combat.Skill1.performed -= OnSkill1;
        controls.Combat.Skill2.performed -= OnSkill2;
        controls.Combat.Skill3.performed -= OnSkill3;
        controls.Combat.Skill4.performed -= OnSkill4;
        controls.Combat.Skill5.performed -= OnSkill5;

        controls.Disable();
    }

    void OnSkill1(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        Debug.Log("Input: Skill1 pressionada");
        hotbar.UseSlot(0);
    }

    void OnSkill2(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        Debug.Log("Input: Skill2 pressionada");
        hotbar.UseSlot(1);
    }

    void OnSkill3(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        Debug.Log("Input: Skill3 pressionada");
        hotbar.UseSlot(2);
    }

    void OnSkill4(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        Debug.Log("Input: Skill4 pressionada");
        hotbar.UseSlot(3);
    }

    void OnSkill5(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        Debug.Log("Input: Skill5 pressionada");
        hotbar.UseSlot(4);
    }
}
