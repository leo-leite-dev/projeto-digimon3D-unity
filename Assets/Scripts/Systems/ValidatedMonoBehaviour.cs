using UnityEngine;

public abstract class ValidatedMonoBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        Validate();
        OnAwake();
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        Validate();
    }
#endif

    protected virtual void Validate() { }

    protected virtual void OnAwake() { }

    protected T GetRequired<T>(T current = null)
        where T : Component
    {
        if (current != null)
            return current;

        var comp = GetComponent<T>() ?? GetComponentInParent<T>();

        if (comp == null)
            Debug.LogError($"{GetType().Name} não encontrou {typeof(T).Name}", this);

        return comp;
    }
}
