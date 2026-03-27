using UnityEngine;

public class DigimonFollowReferences : MonoBehaviour
{
    [SerializeField]
    private DigimonFollow follow;

    public DigimonFollow Follow => follow;

    public bool IsValid()
    {
        if (follow == null)
        {
            Debug.LogError("❌ FollowReferences: follow missing", this);
            return false;
        }

        return true;
    }
}
