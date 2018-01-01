namespace TCGCards.Core
{
    public interface IAbility
    {
        void Activate(Player owner, Player opponent);

        void SetTarget(IPokemonCard target);

        TriggerType TriggerType { get; set; }

        IPokemonCard Owner { get; }
    }
}
