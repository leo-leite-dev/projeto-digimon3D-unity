using UnityEngine;

public class DigimonEnemyController : MonoBehaviour
{
    private IEnemyBehaviour wander;
    private IEnemyBehaviour currentBehaviour;

    private DigimonAttack attack;

    private EnemyState currentState;

    public bool IsInCombat => currentState == EnemyState.InCombat;

    public void Inject(DigimonAttack atk)
    {
        if (atk == null)
        {
            Debug.LogError("❌ Attack não injetado no EnemyController", this);
            return;
        }

        attack = atk;
    }

    private void Awake()
    {
        wander = GetComponent<EnemyWander>();
    }

    private void Start()
    {
        ActivateInitialState();
    }

    private void ActivateInitialState()
    {
        ChangeState(EnemyState.Patrol, wander);
    }

    public void EnterCombat()
    {
        if (currentState == EnemyState.InCombat)
            return;

        ChangeState(EnemyState.InCombat, null);

        Debug.Log("🔥 Enemy entrou em combate");
    }

    private void ChangeState(EnemyState newState, IEnemyBehaviour newBehaviour)
    {
        if (currentState == newState)
            return;

        Debug.Log($"🧠 Enemy State: {currentState} → {newState}");

        currentState = newState;

        SwitchTo(newBehaviour);
    }

    private void SwitchTo(IEnemyBehaviour next)
    {
        if (currentBehaviour == next)
            return;

        currentBehaviour?.Deactivate();

        currentBehaviour = next;

        currentBehaviour?.Activate();
    }
}
