public static class EvolutionCalculator
{
    public static bool CanEvolve(Digimon digimon, DigimonStage nextStage)
    {
        int level = digimon.level.Level;

        switch (nextStage)
        {
            case DigimonStage.InTraining:
                return level >= 5;
            case DigimonStage.Rookie:
                return level >= 10;
            case DigimonStage.Champion:
                return level >= 25;
            case DigimonStage.Ultimate:
                return level >= 50;
            case DigimonStage.Mega:
                return level >= 75;
            case DigimonStage.Jogress:
                return level >= 90;
        }

        return false;
    }
}
