namespace TCGCards.Core
{
    public enum GameFieldState
    {
        WaitingForConnection,
        WaitingForRegistration,
        BothSelectingActive,
        BothSelectingBench,
        InTurn,
        GameOver,
        Attacking,
        TurnEnding,
        PostAttack,
        AbilitySpecial
    }
}
