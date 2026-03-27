using System;

[Serializable]
public class DigimonLevel
{
    public const int MAX_LEVEL = 100;

    public int Level = 1;
    public int Experience = 0;

    public int AttributePoints = 0;

    public int ExpToNextLevel => Level * 100;

    public bool AddExperience(int amount)
    {
        Experience += amount;

        if (Experience >= ExpToNextLevel)
        {
            Experience -= ExpToNextLevel;
            Level++;

            AttributePoints += 5;

            return true;
        }

        return false;
    }

    public void LevelUp()
    {
        Level++;
        AttributePoints += 5;
    }
}
