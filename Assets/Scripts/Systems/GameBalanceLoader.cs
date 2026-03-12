using UnityEngine;

public class GameBalanceLoader : MonoBehaviour
{
    public ExperienceBalance experienceBalance;

    void Awake()
    {
        ExperienceCalculator.balance = experienceBalance;
    }
}
