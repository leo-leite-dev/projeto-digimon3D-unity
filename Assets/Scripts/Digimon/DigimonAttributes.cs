using System;

[Serializable]
public class DigimonAttributes
{
    public int Strength;
    public int Intelligence;
    public int Agility;
    public int Vitality;
    public int Spirit;

    public void AddStrength(int amount) => Strength += amount;

    public void AddIntelligence(int amount) => Intelligence += amount;

    public void AddAgility(int amount) => Agility += amount;

    public void AddVitality(int amount) => Vitality += amount;

    public void AddSpirit(int amount) => Spirit += amount;
}
