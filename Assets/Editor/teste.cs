using UnityEngine;
using UnityEditor;

public class FixAnimationScale
{
    [MenuItem("Tools/Digimon/Fix Animation Scale")]
    static void FixScale()
    {
        string folder = "Assets/Digimons/GabumonLine/Animations/01_Rockie";

        string[] guids = AssetDatabase.FindAssets("t:AnimationClip", new[] { folder });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);

            RemoveScaleCurves(clip);

            EditorUtility.SetDirty(clip);

            Debug.Log("Scale removida de: " + clip.name);
        }

        AssetDatabase.SaveAssets();

        Debug.Log("✔ Todas as animações foram corrigidas!");
    }

    static void RemoveScaleCurves(AnimationClip clip)
    {
        var bindings = AnimationUtility.GetCurveBindings(clip);

        foreach (var binding in bindings)
        {
            if (binding.propertyName.Contains("m_LocalScale"))
            {
                AnimationUtility.SetEditorCurve(clip, binding, null);
            }
        }
    }
}