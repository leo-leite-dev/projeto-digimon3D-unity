public struct WanderContext
{
    public WanderArea WanderArea;
    public DigimonMovement Movement;
    public DigimonAnimator Animator;
    public DigimonEnemyView View;
    public DigimonEnemyController Controller;

    public WanderContext(
        WanderArea wanderArea,
        DigimonMovement movement,
        DigimonAnimator animator,
        DigimonEnemyView view,
        DigimonEnemyController controller
    )
    {
        WanderArea = wanderArea;
        Movement = movement;
        Animator = animator;
        View = view;
        Controller = controller;
    }
}
