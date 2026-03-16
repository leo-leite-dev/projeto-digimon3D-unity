using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private HotbarController hotbar;

    [SerializeField]
    private TargetSystem targetSystem;

    [SerializeField]
    private Camera targetCamera;

    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();

        if (hotbar == null)
            TryGetComponent(out hotbar);

        if (targetSystem == null)
            TryGetComponent(out targetSystem);

        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void OnEnable()
    {
        controls.Enable();
        RegisterCombatInputs();
    }

    void OnDisable()
    {
        UnregisterCombatInputs();
        controls.Disable();
    }

    void RegisterCombatInputs()
    {
        controls.Combat.Skill1.performed += OnSkill1;
        controls.Combat.Skill2.performed += OnSkill2;
        controls.Combat.Skill3.performed += OnSkill3;
        controls.Combat.Skill4.performed += OnSkill4;
        controls.Combat.Skill5.performed += OnSkill5;
        controls.Combat.NextTarget.performed += OnNextTarget;
        controls.Combat.ClickTarget.performed += OnClickTarget;
    }

    void UnregisterCombatInputs()
    {
        controls.Combat.Skill1.performed -= OnSkill1;
        controls.Combat.Skill2.performed -= OnSkill2;
        controls.Combat.Skill3.performed -= OnSkill3;
        controls.Combat.Skill4.performed -= OnSkill4;
        controls.Combat.Skill5.performed -= OnSkill5;
        controls.Combat.NextTarget.performed -= OnNextTarget;
        controls.Combat.ClickTarget.performed -= OnClickTarget;
    }

    void OnSkill1(InputAction.CallbackContext ctx)
    {
        if (hotbar == null)
            return;

        Debug.Log("Input: Skill1 pressionada");
        hotbar.UseSlot(0);
    }

    void OnSkill2(InputAction.CallbackContext ctx)
    {
        if (hotbar == null)
            return;

        Debug.Log("Input: Skill2 pressionada");
        hotbar.UseSlot(1);
    }

    void OnSkill3(InputAction.CallbackContext ctx)
    {
        if (hotbar == null)
            return;

        Debug.Log("Input: Skill3 pressionada");
        hotbar.UseSlot(2);
    }

    void OnSkill4(InputAction.CallbackContext ctx)
    {
        if (hotbar == null)
            return;

        Debug.Log("Input: Skill4 pressionada");
        hotbar.UseSlot(3);
    }

    void OnSkill5(InputAction.CallbackContext ctx)
    {
        if (hotbar == null)
            return;

        Debug.Log("Input: Skill5 pressionada");
        hotbar.UseSlot(4);
    }

    void OnNextTarget(InputAction.CallbackContext ctx)
    {
        if (targetSystem == null)
            return;

        Debug.Log("Input: próximo alvo");
        targetSystem.CycleTarget();
    }

    void OnClickTarget(InputAction.CallbackContext ctx)
    {
        if (targetSystem == null)
            return;

        TrySelectTargetByClick();
    }

    void TrySelectTargetByClick()
    {
        if (targetCamera == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = targetCamera.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        if (!hit.collider.CompareTag("Enemy"))
            return;

        GameObject enemy = hit.collider.transform.root.gameObject;

        if (targetSystem.TrySetTarget(enemy))
            Debug.Log($"Input: alvo selecionado por clique -> {enemy.name}");
    }
}
