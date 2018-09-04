using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Fling : Attack
    {
        public Fling()
        {
            Name = "Fling";
            Description = "Your opponent shuffles his or her Active Pokémon and all cards attached to it into his or her deck. This attack can't be used if your opponent has no Benched Pokémon.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 3),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            opponent.Deck.ShuffleInCard(opponent.ActivePokemonCard);
            opponent.Deck.ShuffleInCards(opponent.ActivePokemonCard.AttachedEnergy.OfType<ICard>());

            if (opponent.BenchedPokemon.Count == 1)
            {
                opponent.SetActivePokemon(opponent.BenchedPokemon.First());
            }
            else
            {
                game.StateQueue.Enqueue(GameFieldState.UnActivePlayerSelectingFromBench);
            }
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {       
            return 0;
        }

        public override bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            return base.CanBeUsed(game, owner, opponent) && opponent.BenchedPokemon.Any();
        }
    }
}
