using UnityEditor;

[CustomEditor(typeof(DigimonData))]
public class DigimonDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DigimonData data = (DigimonData)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Calculated Stats", EditorStyles.boldLabel);

        DigimonStats stats = new DigimonStats();
        DigimonStatsCalculator.CalculateStats(data.attributes, stats);

        EditorGUI.BeginDisabledGroup(true);

        EditorGUILayout.IntField("HP", stats.Hp);
        EditorGUILayout.IntField("Physical Attack", stats.PhysicalAttack);
        EditorGUILayout.IntField("Magic Attack", stats.MagicAttack);
        EditorGUILayout.IntField("Physical Defense", stats.PhysicalDefense);
        EditorGUILayout.IntField("Magic Defense", stats.MagicDefense);
        EditorGUILayout.IntField("Speed", stats.Speed);

        EditorGUILayout.FloatField("Crit Chance", stats.CritChance);
        EditorGUILayout.FloatField("Crit Damage", stats.CritDamage);
        EditorGUILayout.FloatField("Evasion", stats.Evasion);

        EditorGUI.EndDisabledGroup();
    }
}
