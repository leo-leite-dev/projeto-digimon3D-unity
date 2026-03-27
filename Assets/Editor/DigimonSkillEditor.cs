using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DigimonSkill))]
public class DigimonSkillEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var skillTypeProp = serializedObject.FindProperty("skillType");
        var useDelayedProp = serializedObject.FindProperty("useDelayedMovement");

        EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillName"));

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Core", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("range"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cooldown"));

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Type", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(skillTypeProp);

        EditorGUILayout.Space();

        if ((SkillType)skillTypeProp.enumValueIndex == SkillType.Effect)
        {
            EditorGUILayout.LabelField("Effect", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("effectPrefab"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("projectileSpeed"));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(useDelayedProp, new GUIContent("Use Delayed Movement"));

            if (useDelayedProp.boolValue)
            {
                EditorGUILayout.HelpBox(
                    "Movement será controlado por Animation Event (OnActivateProjectile).",
                    MessageType.Info
                );

                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty("initialMovement"),
                    new GUIContent("Initial Movement")
                );

                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty("delayedMovement"),
                    new GUIContent("Delayed Movement")
                );
            }
            else
            {
                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty("projectileMovement"),
                    new GUIContent("Projectile Movement")
                );
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Timing", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("damageDelay"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lifeTime"));
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("animationTrigger"));

        serializedObject.ApplyModifiedProperties();
    }
}
