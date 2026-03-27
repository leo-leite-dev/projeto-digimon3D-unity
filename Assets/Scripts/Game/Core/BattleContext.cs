using System;

public class BattleContext
{
    public Digimon Player { get; }
    public Digimon Enemy { get; }

    public BattleState State { get; private set; }

    public event Action<BattleState> OnBattleFinished;

    public BattleContext(Digimon player, Digimon enemy)
    {
        Player = player;
        Enemy = enemy;

        State = BattleState.Running;
    }

    public void SetState(BattleState state)
    {
        if (State == state)
            return;

        State = state;

        if (State == BattleState.Victory || State == BattleState.Defeat)
            OnBattleFinished?.Invoke(State);
    }

    public bool IsRunning => State == BattleState.Running;
    public bool IsFinished => !IsRunning;

    public Digimon GetOpponent(Digimon digimon)
    {
        if (digimon == Player)
            return Enemy;
        if (digimon == Enemy)
            return Player;

        return null;
    }
}
