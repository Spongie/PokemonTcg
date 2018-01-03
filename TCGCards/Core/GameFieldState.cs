namespace TCGCards.Core
{
    public enum GameFieldState
    {
        WaitingForConnection,
        WaitingForRegistration,
        SelectingActive,
        SelectingBench,
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
