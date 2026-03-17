using UnityEngine;

public static class DigimonVisualReferencesResolver
{
    public static void Refresh(DigimonReferences references)
    {
        Transform modelRoot = TransformSearchUtil.FindChildRecursive(
            references.transform,
            "ModelRoot"
        );

        Transform searchRoot = modelRoot != null ? modelRoot : references.transform;

        Animator animator = searchRoot.GetComponentInChildren<Animator>(true);

        references.SetVisualInternal(modelRoot, animator);
    }
}