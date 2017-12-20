using System.Collections.Generic;
using Assets.Scripts.Game;

namespace Assets.Scripts.Cards.PokemonCards.TeamRocket.Attacks
{
    public class Flop : Attack
    {
        public Flop()
        {
            Name = "Flop";
            Description = string.Empty;
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return GetDamageAfterResistanceAndWeakness(10, owner.ActivePokemonCard, opponent.ActivePokemonCard);
        }
    }
}
