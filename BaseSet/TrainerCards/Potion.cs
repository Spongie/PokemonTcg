using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.TrainerCards
{
    public class Potion : TrainerCard
    {
        public Potion(Player owner) : base(owner)
        {
            Name = "Potion";
            Description = "Remove up to 2 damage counters from 1 of your Pokémon.";
            Set = Singleton.Get<Set>();
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourPokemonMessage("Select pokemon to heal 20").ToNetworkMessage(game.Id));

            var pokemon = (PokemonCard)game.FindCardById(response.Cards.First());

            pokemon.DamageCounters -= 20;

            if (pokemon.DamageCounters < 0)
            {
                pokemon.DamageCounters = 0;
            }
        }
    }
}
