using UnityEngine;

public interface IAttackCapability : IDigimonCapability
{
    void UseSkill(DigimonSkill skill, GameObject target);
    bool CanUseSkill(DigimonSkill skill, GameObject target);
}
