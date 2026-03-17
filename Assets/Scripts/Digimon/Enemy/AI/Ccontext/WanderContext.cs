public struct WanderContext
{
    public WanderArea WanderArea;
    public EnemySpawner Spawner;

    public WanderContext(WanderArea wanderArea, EnemySpawner spawner)
    {
        WanderArea = wanderArea;
        Spawner = spawner;
    }
}
