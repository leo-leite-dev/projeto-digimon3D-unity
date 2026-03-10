public static class CalculatorMatrix
{
    static readonly float[,] elementMatrix =
    {
        { 1f, 0.5f, 1.5f, 1f, 1f, 1f, 1f, 1f },
        { 1.5f, 1f, 1f, 1f, 1f, 1f, 1f, 1f },
        { 1f, 1f, 1f, 1.5f, 1f, 1f, 1f, 1f },
        { 1f, 1f, 1f, 1f, 1.5f, 1f, 1f, 1f },
        { 1f, 1f, 1f, 1f, 1f, 1.5f, 1f, 1f },
        { 1f, 1.5f, 1f, 1f, 1f, 1f, 1f, 1f },
        { 1f, 1f, 1f, 1f, 1f, 1f, 1.5f, 0.5f },
        { 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 1.5f },
    };

    static readonly float[,] typeMatrix =
    {
        { 1f, 1f, 1f, 1f, 1f },
        { 1f, 1f, 0.5f, 1.5f, 1f },
        { 1f, 1.5f, 1f, 0.5f, 1f },
        { 1f, 0.5f, 1.5f, 1f, 1f },
        { 1f, 1f, 1f, 1f, 1f },
    };

    public static float GetElementModifier(DigimonElement attacker, DigimonElement defender)
    {
        if (attacker == DigimonElement.None || defender == DigimonElement.None)
            return 1f;

        int atk = (int)attacker - 1;
        int def = (int)defender - 1;

        return elementMatrix[atk, def];
    }

    public static float GetTypeModifier(DigimonType attacker, DigimonType defender)
    {
        return typeMatrix[(int)attacker, (int)defender];
    }
}
