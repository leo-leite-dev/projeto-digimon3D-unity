using UnityEngine;

public class DigimonFollowOverworldComposer : MonoBehaviour
{
    public void Compose(
        DigimonCoreReferences core,
        DigimonFollowReferences followRefs,
        Transform player,
        Transform followPoint
    )
    {
        if (!ValidateAll(core, followRefs, player, followPoint))
            return;

        var go = core.gameObject;

        var digimonAnimator = DigimonVisualComposer.SetupVisual(core, null, go);

        if (digimonAnimator == null)
            return;

        DigimonVisualComposer.BindMovementAnimator(core, go);

        followRefs.Follow.Inject(core.Movement, digimonAnimator);

        SetupMovement(core);
        SetupFollow(followRefs, player, followPoint);

        Debug.Log("[DigimonFollowOverworldComposer] ✅ COMPOSED");
    }

    private void SetupMovement(DigimonCoreReferences core)
    {
        if (core.Movement == null || !core.Movement.IsAgentReady)
            Debug.LogError("❌ NavMeshAgent não está pronto no Movement", this);
    }

    private void SetupFollow(
        DigimonFollowReferences followRefs,
        Transform player,
        Transform followPoint
    )
    {
        var follow = followRefs.Follow;

        if (follow == null)
        {
            Debug.LogError("❌ DigimonFollow missing", this);
            return;
        }

        follow.Initialize(player, followPoint);
    }

    private bool ValidateAll(
        DigimonCoreReferences core,
        DigimonFollowReferences followRefs,
        Transform player,
        Transform followPoint
    )
    {
        bool valid = true;

        if (core == null || !core.IsValid())
        {
            Debug.LogError("❌ CoreReferences inválido", this);
            valid = false;
        }

        if (followRefs == null || !followRefs.IsValid())
        {
            Debug.LogError("❌ FollowReferences inválido", this);
            valid = false;
        }

        if (player == null)
        {
            Debug.LogError("❌ Player não fornecido", this);
            valid = false;
        }

        if (followPoint == null)
        {
            Debug.LogError("❌ FollowPoint não fornecido", this);
            valid = false;
        }

        return valid;
    }
}
