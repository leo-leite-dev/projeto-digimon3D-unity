using System.Collections.Generic;
using UnityEngine;

public class DigimonCapabilityProvider : MonoBehaviour
{
    private Dictionary<System.Type, IDigimonCapability> capabilities;

    private void Awake()
    {
        capabilities = new Dictionary<System.Type, IDigimonCapability>();

        var all = GetComponents<IDigimonCapability>();

        foreach (var cap in all)
        {
            capabilities[cap.GetType().GetInterfaces()[0]] = cap;
        }
    }

    public T Get<T>()
        where T : class, IDigimonCapability
    {
        if (capabilities.TryGetValue(typeof(T), out var cap))
            return cap as T;

        return null;
    }
}
