using Entities;

namespace TCGCards.Core.Abilities
{
    public class CinnabarCityAbility : Ability, IResistanceModifier
    {
        public int GetModifiedResistance(PokemonCard attacker, PokemonCard defender)
        {
            if (defender.Name.Contains("Blaine") && defender.Type == EnergyTypes.Water)
            {
                return 0;
            }

            return defender.ResistanceAmount;
        }
    }
}
