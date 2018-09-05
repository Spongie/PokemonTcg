namespace TCGCards.Core
{
    public enum GameFieldState
    {
        WaitingForConnection,
        WaitingForRegistration,
        BothSelectingActive,
        BothSelectingBench,
        TurnStarting,
        ActivePlayerSelectingFromBench,
        UnActivePlayerSelectingFromBench,
        ActivePlayerSelectingPrize,
        UnActivePlayerSelectingPrize,
        InTurn,
        WaitingForDeckSearch,
        EndAttack
    }
}
