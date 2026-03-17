public class DigimonSpawnPipeline
{
    public void Compose(DigimonEnemy enemy, DigimonData data, WanderContext context)
    {
        if (enemy == null)
            return;

        enemy.Setup(data);

        var composables = enemy.GetComponents<IDigimonComposable>();
        foreach (var c in composables)
            c.Compose();

        var initializables = enemy.GetComponents<IContextInitializable<WanderContext>>();
        foreach (var i in initializables)
            i.Initialize(context);
    }
}
