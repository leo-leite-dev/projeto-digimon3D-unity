using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    [Header("=== Gameplay Components ===")]
    [SerializeField]
    private PlayerMovement movement;

    [Header("=== Scene References ===")]
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private Transform followPoint;

    public PlayerMovement Movement => movement;

    public Transform SpawnPoint => spawnPoint;
    public Transform FollowPoint => followPoint;

    public bool IsValid()
    {
        bool valid = true;

        void Check(Object obj, string name)
        {
            if (obj == null)
            {
                Debug.LogError($"❌ PlayerReferences: {name} missing", this);
                valid = false;
            }
        }

        Check(movement, nameof(movement));
        Check(spawnPoint, nameof(spawnPoint));
        Check(followPoint, nameof(followPoint));

        return valid;
    }

#if UNITY_EDITOR
    [ContextMenu("Validate References")]
    private void ValidateInEditor()
    {
        IsValid();
    }
#endif
}
