// using UnityEngine;

// public class SkillEffectInitializer
// {
//     public void Initialize(
//         GameObject obj,
//         DigimonSkill skill,
//         Transform target,
//         Digimon attacker,
//         DigimonAttack attack
//     )
//     {
//         if (obj == null)
//         {
//             Debug.LogError("Objeto do efeito é nulo.");
//             return;
//         }

//         if (skill == null)
//         {
//             Debug.LogError("Skill é nula.");
//             return;
//         }

//         if (attacker == null)
//         {
//             Debug.LogError("Attacker é nulo.");
//             return;
//         }

//         InitializeEffect(obj, skill, target, attacker, attack);
//         InitializeProjectile(obj, skill, target, attacker, attack);
//     }

//     private void InitializeEffect(
//         GameObject obj,
//         DigimonSkill skill,
//         Transform target,
//         Digimon attacker,
//         DigimonAttack attack
//     )
//     {
//         var effect = obj.GetComponent<SkillEffect>();

//         if (effect == null)
//             return;

//         effect.Setup(skill, target, attacker, attack);
//     }

//     private void InitializeProjectile(
//         GameObject obj,
//         DigimonSkill skill,
//         Transform target,
//         Digimon attacker,
//         DigimonAttack attack
//     )
//     {
//         var projectile = obj.GetComponent<SkillProjectile>();

//         if (projectile == null)
//             return;

//         if (target == null)
//         {
//             Debug.LogWarning("SkillProjectile sem target.");
//             return;
//         }

//         var defender = target.GetComponent<Digimon>();

//         if (defender == null)
//         {
//             Debug.LogError("Target não possui Digimon.");
//             return;
//         }

//         projectile.Initialize(skill, attacker, defender, attack);
//     }
// }
