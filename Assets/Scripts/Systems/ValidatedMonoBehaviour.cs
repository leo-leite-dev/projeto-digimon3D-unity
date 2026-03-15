using UnityEngine;

public abstract class ValidatedMonoBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        Validate();
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        Validate();
    }
#endif

    protected virtual void Validate() { }
}
