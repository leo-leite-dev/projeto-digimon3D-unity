using UnityEngine;

public struct CombatContext
{
    public Vector3 Origin;
    public Vector3 Direction;

    public GameObject SuggestedTarget;

    public CombatContext(Vector3 origin, Vector3 direction, GameObject suggestedTarget = null)
    {
        Origin = origin;
        Direction = direction;
        SuggestedTarget = suggestedTarget;
    }
}
