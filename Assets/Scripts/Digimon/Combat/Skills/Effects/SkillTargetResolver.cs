using UnityEngine;

public class SkillTargetResolver
{
    public Transform Resolve(GameObject target)
    {
        if (target == null)
        {
            Debug.LogError("❌ Target null no resolver");
            return null;
        }

        var receiver = target.GetComponentInChildren<DigimonHitReceiver>();

        if (receiver == null)
        {
            Debug.LogError($"❌ DigimonHitReceiver não encontrado em: {target.name}");
            return null;
        }

        return receiver.transform;
    }
}
