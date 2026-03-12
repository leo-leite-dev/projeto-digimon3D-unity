using UnityEngine;

[CreateAssetMenu(menuName = "Digimon/Skill")]
public class DigimonSkill : ScriptableObject
{
    public string skillName;

    public AttackType attackType;
    public AttackRange rangeType;

    public float range = 3f;
    public int damage = 3;

    public float cooldown = 1f;

    public GameObject projectilePrefab;
}
