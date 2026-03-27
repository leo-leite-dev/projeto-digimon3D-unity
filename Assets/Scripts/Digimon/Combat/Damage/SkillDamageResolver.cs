using UnityEngine;

public class SkillDamageResolver : MonoBehaviour
{
    public bool TryBuildHitContext(
        DigimonSkill skill,
        Transform target,
        Digimon attacker,
        out HitContext context
    )
    {
        context = default;

        if (skill == null || target == null || attacker == null)
        {
            Debug.LogWarning("⚠️ TryBuildHitContext abortado: skill/target/attacker null");
            return false;
        }

        Debug.Log($"🎯 TARGET RECEBIDO NO DAMAGE: {target.name}");

        var defender = target.root.GetComponent<Digimon>();

        if (defender == null)
        {
            Debug.LogError($"❌ DEFENDER NULL (root) para target: {target.name}");
            return false;
        }

        Debug.Log($"✅ DEFENDER ENCONTRADO: {defender.name}");

        int damage = CombatCalculator.CalculateDamage(attacker, defender, skill);

        Debug.Log($"💢 DAMAGE CALCULADO: {damage}");

        context = new HitContext
        {
            FinalDamage = damage,
            IsCritical = false,
            Attacker = attacker,
            Defender = defender,
        };

        Debug.Log("🔥 HitContext CRIADO COM SUCESSO");

        return true;
    }
}
