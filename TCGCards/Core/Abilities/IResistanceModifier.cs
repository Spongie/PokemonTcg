namespace TCGCards.Core.Abilities
{
    public interface IResistanceModifier 
    {
        int GetModifiedResistance(PokemonCard attacker, PokemonCard defender);
    }
}
