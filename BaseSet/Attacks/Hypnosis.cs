using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Hypnosis : Attack
    {
        public Hypnosis()
        {
            Name = "Hypnosis";
            Description = "The Defending Pokémon is now Asleep.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            game.GameLog.AddMessage(opponent.ActivePokemonCard.GetName() + " is now asleep");
            opponent.ActivePokemonCard.IsAsleep = true;
        }
    }
}
